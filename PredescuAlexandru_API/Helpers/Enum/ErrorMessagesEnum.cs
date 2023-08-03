namespace PredescuAlexandru_API.Helpers.Enum
{
    public static class ErrorMessagesEnum
    {
        public static class Announcement
        {
            public const string NoFound = "Nu a fost gasit in tabel";

            public const string NoFoundById = "Nu a fost gasit dupa id";

            public const string BadRequest = "Formatul transmis nu este corect";

            public const string ZeroUpdateToSave = "Nu sunt modificari pe anunt pentru a fi salvat";

            public const string StartEndDatesError = "Data de inceput nu poate fi dupa data de sfarsit";

            public const string TitleExistError = "Titlu exista in baza de date";

        }
    }
}

