
namespace SoundJabber.ViewModels
{
    public class SoundModel
    {
        public SoundGroup Animals { get; set; }
        public SoundGroup Cartoons { get; set; }
        public SoundGroup Warnings { get; set; }
        public SoundGroup CustomSounds { get; set; }
        public SoundGroup Taunts { get; set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            Animals = CreateAnimalsGroup();
            Cartoons = CreateCartoonsGroup();
            Taunts = CreateTauntsGroup();
            Warnings = CreateWarningsGroup();
            CustomSounds = CreateCustomSoundsGroup();

            IsDataLoaded = true;
        }

        private SoundGroup CreateCustomSoundsGroup()
        {
            throw new System.NotImplementedException();
        }

        private SoundGroup CreateWarningsGroup()
        {
            throw new System.NotImplementedException();
        }

        private SoundGroup CreateTauntsGroup()
        {
            throw new System.NotImplementedException();
        }

        private SoundGroup CreateCartoonsGroup()
        {
            throw new System.NotImplementedException();
        }

        private SoundGroup CreateAnimalsGroup()
        {
            throw new System.NotImplementedException();
        }
    }
}
