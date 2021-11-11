namespace SimplePomodoro.Localization
{
    public static class Translator
    {
        public static string Translate(string key) => Language.ResourceManager.GetString(key);
    }
} 