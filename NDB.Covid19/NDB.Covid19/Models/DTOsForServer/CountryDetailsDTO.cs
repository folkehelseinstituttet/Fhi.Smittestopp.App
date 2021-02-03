namespace NDB.Covid19.Models.DTOsForServer
{
    public class CountryDetailsDTO
    {
        public string Name_NB { get; set; }
        public string Name_NN { get; set; }
        public string Name_EN { get; set; }
        public string Code { get; set; }

        public string GetName()
        {
            string language = LocalesService.GetLanguage();

            switch (language)
            {
                case "nb":
                    return Name_NB;
                case "nn":
                    return Name_NN;
                case "en":
                    return Name_EN;
                default:
                    return Name_EN;
            }
        }
    }
}
