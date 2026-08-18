using System.Collections.Generic;
using BH.SDK.Interop.AfterBeat.Models;

namespace BH.SDK.Interop.AfterBeat
{
    // Afterbeat ships 21 themes with the game, and a level that uses one of them stores NOTHING but
    // its index: the theme track holds the string "19" and themes[] stays empty. Read against the
    // file alone that is a dangling reference, so every level that never authored a palette of its
    // own imported with no theme at all - a whole level of colours resolving to white.
    //
    // So the table lives here. It is not a preset tier of this project's own (there is none - see
    // the root CLAUDE.md, "The texture preset tier is gone"): a default theme referenced by a level
    // is MATERIALIZED into that level's own Resources.Themes as an ordinary custom theme, which is
    // what makes the level self-contained afterwards and what makes the export direction trivial -
    // it writes the copy back out as a custom theme like any other.
    //
    // The colours were MEASURED off the shipped game (DataManager.BeatmapThemes, serialized into
    // Afterbeat_Data/level2) rather than transcribed from the wiki or from Interop/AB-DEFAULT-
    // THEMES.md - that document covers only the last 11 of the 21 and had two colours wrong.
    //
    // Object colours are the one list whose length VARIES between themes (2 on Black/White, 9 on
    // every theme from Desert Heat on). The short ones are stored short, exactly as the game holds
    // them; the slots past the end keep ThemeData's own white, since the source game has no colour
    // there to copy either.

    /// <summary> The 21 themes Afterbeat ships with, addressed by the index its levels store. </summary>
    public static class ABDefaultThemes
    {
        /// <summary> How many themes the source game ships; ids run "0" through "20". </summary>
        public const int Count = 21;

        // One line per theme rather than 21 object initializers: this is a transcription of another
        // program's data, it is read as a table, and 714 colour literals spread over 200 lines of
        // initializers is a table nobody can check against the source. The separators are the ones
        // the source document uses - '|' between fields, spaces between colours.

        /// <summary> id | name | gui | background | accent | players | objects | parallax | effects </summary>
        private static readonly string[] Table =
        {
            "0|Machine|212121|94D8DB|EF5350|E57373 64B5F6 81C784 FFB74D|C0ACE1 F17BB8 2F426F 1B1B1C EFEBEF|111111 111111 111111 111111 111111 111111 111111 111111 111111|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "1|Anarchy|212121|FFFFFF|EF5350|E57373 64B5F6 81C784 FFB74D|FFE7E7 C0ACE1 F17BB8 2F426D 4076DF 6CCBCF 1B1B1C EFEBEF|111111 111111 111111 111111 111111 111111 111111 111111 111111|111111 111111 111111 111111 111111 111111 111111 111111 111111",
            "2|Day Night|212121|FFFFFF|EF5350|F44336 2196F3 4CAF50 FF9800|132036 3E5376 A5DCE9 EF8E46 FFCD86 FFF5D5|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "3|Donuts|212121|FFFFFF|EF5350|F44336 2196F3 4CAF50 FF9800|FF86A3 FF6386 FFDF91 FFBF7A 8CEEFF 20DAF5 EC9D75 CE6852 1B1B1C|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "4|Classic|212121|FFFFFF|EF5350|F44336 2196F3 4CAF50 FF9800|346188 517A9E FF6D8B C2415F 1B1B1C EFEBEF|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "5|New|212121|FFFFFF|EF5350|F44336 2196F3 4CAF50 FF9800|1395BA 0D3C55 C02E1D F16C20 EBC844 A2B86C 1B1B1C EFEBEF|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "6|Dark|EFEBEF|030436|EF5350|F44336 2196F3 4CAF50 FF9800|20E4ED 71DF4F F55A75 FEFEFE 1B1B1C|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "7|Black/White|EFEBEF|111111|EF5350|F44336 2196F3 4CAF50 FF9800|FAFAFA FFFFFF|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "8|White/Black|212121|FAFAFA|EF5350|F44336 2196F3 4CAF50 FF9800|222222 333333 444444|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "9|Poison|F7F7F7|3A3A58|EF5350|F44336 2196F3 4CAF50 FF9800|66E0FF FF7C96 6D6E99|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7|F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7 F7F7F7",
            "10|Desert Heat|111111|FCE8C7|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|F7B846 FF6B45 ED5052 ED4F5D 7F492F 68392F 41231F 301B1A FFFFFF|FDE9C6 ECD8B7 DDCAAC C2AC94 A48E79 877163 FDD592 FEC09A F9B8A6|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "11|Ember Stones|F7F7F7|161A19|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|FEDC85 EFAA67 AA674A 2C3A3B 404642 5B5E55 6B6855 A49774 161A19|161A19 0C100F 060A09 2D342D 212723 1C211D 3D3D33 292C25 212320|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "12|FireArmour|111111|F6A410|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|15262A 375C7E 539DAE EED32F EBC22B D6510F CA271C AB1F18 6C100F|F6A410 F56D04 FB4800 FE1F02 F40400 D70103 C10908 921010 6D1916|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "13|Jungle Waterway|111111|F2FAAC|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|BBD52F 8FA42C 455306 2E3C00 2A2B17 6A864C 3C5329 000000 FFFFFF|F2FAAC FBFDCF F7FCBE EFFD8E E4F16F D9EA6A E2F1A0 D8E998 C8D989|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "14|Lure|F7F7F7|0A1D2B|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|21F1FD 0CAFFC 0575FB 0943C8 183C92 FFFD8C F3D457 DDA73D AF7F2D|0A1D2B 132941 142A4A 0F2F48 143D5D 131420 262837 3C3A4F 3F4662|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "15|Shiver|323233|BCDFF8|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|99FFFF 52E1FB 52ADF7 407DDD 0635FF 0419CB 3D7F83 276563 112437|BCDFF8 C6ECFB DAF7FD 86B2EC 7195C0 7D97B3 5E7B98 495E78 32485F|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "16|Starlight|F2F2F2|05132D|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|246B67 3C9885 70C09C 7ACA90 D4EC9B F5FC7F AAE2F1 FFFFFF 05132D|05132D 082336 072A3E 0F384A 184D58 010E17 19533B 003E2E 16403C|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "17|Stone Field|F2F2F2|28323B|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|A36D4B 7A5640 AF9D90 C2A599 DCC2BD B99798 7D5750 603F3C 402425|28323B 313D48 354650 3E5865 637583 77838F 262D33 1E272C 181F25|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "18|Vicious Goop|323233|B3CBB4|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|EC412F FE674E FEAC7C FCD9B3 B7A691 A58B76 83695B 000000 FFFFFF|B3CBB4 ACBFA8 A4B29B 7A8F81 96AC9A A5BCA7 5E8C87 88AB9D 9DBAA8|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "19|Wonderland|323233|FFAAFF|EF5350|FA5C66 5C8BFA 06D6A0 FFD166|FD2FAB 961865 410930 240419 8151AF C860CE FF7DF8 FFAAFF FFFFFF|FFAAFF FEB8FE FAC4FE FF8BFE FF97FE FEA3FF B56BAE D98AD6 ED9AEB|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
            "20|HotPanda|FFFFFF|27232A|C80224|FA5C66 5C8BFA 06D6A0 FFD166|C8024F FF585F FCE4A8 BED7E9 0A7E8B 093A54 2F2041 27232A FFFFFF|FFAAFF FEB8FE FAC4FE FF8BFE FF97FE FEA3FF B56BAE D98AD6 ED9AEB|FF7C96 EF4768 B7003E 6D6E99 41436B 161C40 66E0FF 00AEEF 007FBC",
        };

        private static Dictionary<string, VgtTheme> _byId;

        /// <summary> Positional meaning of one <see cref="Table"/> row. </summary>
        private static class Field
        {
            public const int Id = 0;
            public const int Name = 1;
            public const int Gui = 2;
            public const int Background = 3;
            public const int Accent = 4;
            public const int Players = 5;
            public const int Objects = 6;
            public const int Parallax = 7;
            public const int Effects = 8;
            public const int Count = 9;
        }

        /// <summary> True for an id a level can reference without defining the theme itself. </summary>
        public static bool Contains(string sourceId) => sourceId != null && Map.ContainsKey(sourceId);

        /// <summary> One shipped theme in the source format, so it converts through exactly the
        /// same path a theme read out of a file does. Null for an id nothing ships. </summary>
        public static VgtTheme Get(string sourceId)
            => sourceId != null && Map.TryGetValue(sourceId, out var theme) ? theme : null;

        /// <summary> Every shipped theme, in the order the source game lists them. </summary>
        public static IEnumerable<VgtTheme> All()
        {
            foreach (var row in Table) yield return Get(Split(row)[Field.Id]);
        }

        // Parsed once and handed out shared: a VgtTheme is only ever READ from here (the importer
        // copies out of it into a ThemeData), so a per-call copy would allocate 21 objects and 34
        // strings to hand back the same values.
        private static Dictionary<string, VgtTheme> Map
        {
            get
            {
                if (_byId != null) return _byId;

                _byId = new Dictionary<string, VgtTheme>(Table.Length);
                foreach (var row in Table)
                {
                    var theme = Parse(row);
                    if (theme != null) _byId[theme.Id] = theme;
                }

                return _byId;
            }
        }

        private static VgtTheme Parse(string row)
        {
            var fields = Split(row);
            if (fields.Length != Field.Count) return null;

            return new VgtTheme
            {
                Id = fields[Field.Id],
                Name = fields[Field.Name],
                Gui = fields[Field.Gui],
                Background = fields[Field.Background],
                GuiAccent = fields[Field.Accent],
                Players = Colors(fields[Field.Players]),
                Objects = Colors(fields[Field.Objects]),
                Parallax = Colors(fields[Field.Parallax]),
                Effects = Colors(fields[Field.Effects]),
            };
        }

        private static string[] Split(string row) => row.Split('|');

        private static List<string> Colors(string field)
            => new(field.Split(' '));
    }
}
