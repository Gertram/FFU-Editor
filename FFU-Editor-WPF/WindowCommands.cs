using System.Windows.Input;


namespace FFUEditor
{
    public static class WindowCommands
    {
        public static RoutedCommand ImportCommand { get; } = new RoutedCommand();
        public static RoutedCommand ExportCommand { get; } = new RoutedCommand();
        public static RoutedCommand ShowCharacterCommand { get; } = new RoutedCommand();
        public static RoutedCommand ShowCharacterByCodeCommand { get; } = new RoutedCommand();
        public static RoutedCommand ShowCharacterByIndexCommand { get; } = new RoutedCommand();
        public static RoutedCommand AddCharacterCommand { get; } = new RoutedCommand();
        public static RoutedCommand RedactCharacterCommand { get; } = new RoutedCommand();
        public static RoutedCommand RemoveCharacterCommand { get; } = new RoutedCommand();
        public static RoutedCommand FontSettingsCommand { get; } = new RoutedCommand();
        public static RoutedCommand CharactersListCommand { get; } = new RoutedCommand();
        public static RoutedCommand ModifiableCharactersCommand { get; } = new RoutedCommand();
        public static RoutedCommand ToolsCommand { get; } = new RoutedCommand();
    }
}
