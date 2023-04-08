using System;
using System.Text.RegularExpressions;
using Godot;
using Godot.Collections;
using GodotUtilities;

namespace CarrotFantasy.autoload
{
    [Tool]
    public partial class SoundManager : Node
    {
        private static SoundManager instance;
        public static SoundManager Instance
        {
            get => instance;
        }

        /// <summary>
        /// 默认总线音量，取值范围[0,100]
        /// </summary>
        public static readonly int DefaultBusVolume = 50;

        /// <summary>
        /// bus/1/name = &"BGM"
        /// bus/2/name = &"SoundEffects"
        /// 总线名称配置项的key在tres文件中的命名规则
        /// </summary>
        public static readonly Regex regex = new Regex("^bus/\\d+/name$");

        [Node("BGMPlayer")]
        private AudioStreamPlayer BGMPlayer;

        [Node("Fleeting")]
        private Node fleeting;

        /// <summary>
        /// 音效播放器原型
        /// 在编辑器中将SoundEffectsPrototype的Bus设置好，通过Duplicate方法复制节点来继承Bus属性（其他属性同理），可以减少硬编码（fleetingPlayer.Bus = "SoundEffects";）
        /// </summary>
        [Node("Fleeting/SoundEffectsPrototype")]
        private AudioStreamPlayer SoundEffectsPrototype;

        /// <summary>
        /// 当作一个编辑器按钮使用，用来刷新BusVolumesConfig属性
        /// </summary>
        [Export]
        public bool Refresh
        {
            set
            {
                if (!value)
                    return;
                UpdateBusVolumesConfig();
            }
            get => false;
        }

        [Export]
        public Dictionary<string, int> BusVolumesConfig = new();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();
            SoundManager.instance = this;

            BGMPlayer.Finished += ReplayBGM;
            UpdateBusVolumes();
        }

        public void PlayBGM(string path)
        {
            AudioStream currentStream = BGMPlayer.Stream;
            if (currentStream != null && currentStream.ResourcePath == path && BGMPlayer.Playing)
            {
                return;
            }

            AudioStream audioStream = GD.Load<AudioStream>(path);
            BGMPlayer.Stop();
            BGMPlayer.Stream = audioStream;
            BGMPlayer.Play();
        }

        public void ReplayBGM()
        {
            AudioStream currentStream = BGMPlayer.Stream;
            if (currentStream == null)
            {
                return;
            }
            BGMPlayer.Play();
        }

        public void StopBGM()
        {
            BGMPlayer.Stop();
        }

        public void PlayFleeting(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (SoundEffectsPrototype == null)
            {
                return;
            }
            AudioStream audioStream = GD.Load<AudioStream>(path);
            if (audioStream == null)
            {
                return;
            }
            // 通过复制SoundEffectsPrototype节点的方式让fleetingPlayer可以继承一些SoundEffectsPrototype在编辑器中就设置好的属性
            AudioStreamPlayer fleetingPlayer = SoundEffectsPrototype.Duplicate() as AudioStreamPlayer;
            fleetingPlayer.Finished += () => fleetingPlayer.QueueFree();

            fleetingPlayer.Stream = audioStream;
            fleetingPlayer.Autoplay = true;
            fleeting.AddChildDeferred(fleetingPlayer);
        }

        /// <summary>
        /// 音量百分比转分贝
        /// </summary>
        /// <param name="percentage">音量百分比</param>
        /// <returns>分贝</returns>
        private float ConvertPercentageToDecibels(int percentage)
        {
            // 纠正输入值，合法值范围：[0,100]
            if (percentage < 0)
                percentage = 0;
            if (percentage > 100)
                percentage = 100;

            float scale = 20.0f;
            float divisor = 50.0f;
            return scale * (float)Math.Log10(percentage / divisor);
        }

        /// <summary>
        /// 各个总线的音量配置更新到各个总线上
        /// </summary>
        private void UpdateBusVolumes()
        {
            for (int i = 0; i < AudioServer.BusCount; i++)
            {
                string busName = AudioServer.GetBusName(i);
                if (!BusVolumesConfig.ContainsKey(busName))
                    continue;
                AudioServer.SetBusVolumeDb(i, ConvertPercentageToDecibels(BusVolumesConfig[busName]));
            }
        }

        /// <summary>
        /// 更新总线音量配置
        /// </summary>
        private void UpdateBusVolumesConfig()
        {
            // 获取项目设置：项目设置->常规->音频->总线->默认总线布局
            string path = (string)ProjectSettings.GetSetting("audio/buses/default_bus_layout");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            // 加载默认总线布局的资源文件
            Resource busLayoutResource = GD.Load<Resource>(path);
            if (busLayoutResource == null)
            {
                return;
            }

            // 过滤出默认总线布局中所有的总线名称
            Array<string> array = new();
            foreach (var item in busLayoutResource.GetPropertyList())
            {
                string name = item["name"].As<string>();
                if (string.IsNullOrEmpty(name))
                    continue;

                Match match = regex.Match(name);
                if (!match.Success)
                    continue;

                array.Add(busLayoutResource.Get(name).As<string>());
            }

            // 以从默认总线布局资源文件中读取到的总线为准
            // 剔除掉默认总线布局中没有的布局的配置
            // 新增BusVolumesConfig中没用的总线并赋值为默认总线音量
            Dictionary<string, int> tempBusVolumes = new();
            foreach (var item in array)
            {
                int value = BusVolumesConfig.ContainsKey(item) ? BusVolumesConfig[item] : DefaultBusVolume;
                tempBusVolumes.Add(item, value);
            }
            BusVolumesConfig = tempBusVolumes;
            UpdateBusVolumes();
        }
    }
}
