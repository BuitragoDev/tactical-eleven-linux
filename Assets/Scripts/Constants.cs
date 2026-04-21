using System.Collections.Generic;
using UnityEngine;

namespace TacticalEleven.Scripts
{
    public static class Constants
    {
        // -------------------------------- NOMBRE DE LA BASE DE DATOS ---------------------------------
        public const string DATABASE_NAME = "tacticalElevenDB.db";

        // ----------------------------------- VALOR DE LA MONEDA --------------------------------------
        public const string EURO_NAME = "EUR";
        public const float EURO_VALUE = 1f;
        public const string EURO_SYMBOL = "€";
        public const string POUND_NAME = "GBP";
        public const float POUND_VALUE = 0.87f;
        public const string POUND_SYMBOL = "£";
        public const string DOLLAR_NAME = "USD";
        public const float DOLLAR_VALUE = 1.15f;
        public const string DOLLAR_SYMBOL = "$";

        // ------------------------------- PARÁMETROS VOLUMEN AUDIOMIXER -------------------------------
        public const string MASTER_VOLUME_PARAMETER = "MasterVolumeParameter";
        public const string MUSIC_VOLUME_PARAMETER = "MusicVolumeParameter";
        public const string SFX_VOLUME_PARAMETER = "SFXVolumeParameter";

        // ----------------------------------- NOMBRE DE LOS BOTONES -----------------------------------
        public const string MANAGER_MODE_BUTTON = "manager-mode";
        public const string CAREER_MODE_BUTTON = "career-mode";
        public const string SETTINGS_BUTTON = "settings";
        public const string EDITOR_BUTTON = "editor";
        public const string CARGAR_BUTTON = "cargar-partida";
        public const string CREDITS_BUTTON = "credits-icon";
        public const string EXIT_BUTTON = "exit-icon";
        public const string WEB_BUTTON = "web-icon";
        public const string BACK_BUTTON = "btnVolver";
        public const string CONTINUE_BUTTON = "btnSeguir";

        // -------------------------------- NOMBRE DE ESCENAS DEL JUEGO --------------------------------
        public const string INTRO_SCENE = "Intro";
        public const string MAIN_MENU_SCENE = "MainMenu";
        public const string CREDITS_SCENE = "Credits";
        public const string CREATE_MANAGER_SCENE = "CreateManager";
        public const string TEAM_SELECTION_SCENE = "TeamSelection";
        public const string PRE_SEASON_SCENE = "PreSeason";
        public const string TEAM_OBJECTIVES_SCENE = "TeamObjectives";
        public const string MAIN_SCREEN_SCENE = "MainScreen";
        public const string LOAD_SCREEN_SCENE = "CargarPartida";
        public const string SETTINGS_SCREEN_SCENE = "Settings";

        // -------------------------------------- DEMARCACIONES ----------------------------------------
        public static string RolIdToText(int rol)
        {
            return rol switch
            {
                1 => "POR",
                2 => "LD",
                3 => "LI",
                4 => "DFC",
                5 => "MC",
                6 => "MCD",
                7 => "MCO",
                8 => "ED",
                9 => "EI",
                10 => "DC",
                _ => "N/A"
            };
        }

        // -------------------------------------- NACIONALIDADES ---------------------------------------
        public static List<string> ObtenerTodasLasNacionalidades()
        {
            return new List<string>
            {
                "España", "Albania", "Alemania", "Andorra", "Angola", "Antigua y Barbuda", "Arabia Saudita",
                "Argelia", "Argentina", "Armenia", "Australia", "Austria", "Azerbaiyán", "Bahamas", "Bangladesh",
                "Barbados", "Baréin", "Bélgica", "Belice", "Benín", "Bielorrusia", "Birmania", "Bolivia",
                "Bosnia y Herzegovina", "Botsuana", "Brasil", "Brunéi", "Bulgaria", "Burkina Faso", "Burundi",
                "Bután", "Cabo Verde", "Camboya", "Camerún", "Canadá", "Chad", "Chile", "China", "Chipre",
                "Colombia", "Comoras", "Corea del Norte", "Corea del Sur", "Costa Rica", "Costa de Marfil", "Croacia",
                "Cuba", "Curazao", "Dinamarca", "Dominica", "Ecuador", "Egipto", "El Salvador", "Emiratos Árabes Unidos",
                "Eritrea", "Escocia", "Eslovaquia", "Eslovenia", "Estados Unidos", "Estonia", "Eswatini",
                "Etiopía", "Fiji", "Filipinas", "Finlandia", "Francia", "Gabón", "Gales", "Gambia", "Georgia", "Ghana",
                "Granada", "Grecia", "Guatemala", "Guinea", "Guinea-Bisáu", "Guyana", "Haití", "Honduras", "Hungría",
                "India", "Indonesia", "Inglaterra", "Irak", "Irán", "Irlanda", "Islandia", "Islas Feroe",
                "Islas Marshall", "Islas Salomón", "Israel", "Italia", "Jamaica", "Japón", "Jordania", "Kazajistán",
                "Kenia", "Kirguistán", "Kiribati", "Kosovo", "Kuwait", "Laos", "Lesoto", "Letonia", "Líbano", "Liberia",
                "Libia", "Liechtenstein", "Lituania", "Luxemburgo", "Macedonia del Norte", "Madagascar", "Malasia",
                "Malawi", "Maldivas", "Mali", "Malta", "Moldavia", "Marruecos", "Martinica", "Mauricio", "Mauritania",
                "México", "Micronesia", "Mónaco", "Mongolia", "Montenegro", "Mozambique", "Namibia", "Nauru", "Nepal",
                "Nicaragua", "Níger", "Nigeria", "Noruega", "Nueva Zelanda", "Omán", "Países Bajos", "Pakistán",
                "Palaos", "Panamá", "Papúa Nueva Guinea", "Paraguay", "Perú", "Polonia", "Portugal", "Reino Unido",
                "República Checa", "República del Congo", "República Dominicana", "Ruanda", "Rumanía", "Rusia",
                "Samoa", "San Cristóbal y Nieves", "San Marino", "Santo Tomé y Príncipe", "Senegal", "Serbia",
                "Seychelles", "Sierra Leona", "Singapur", "Siria", "Somalia", "Sri Lanka", "Sudáfrica", "Sudán",
                "Sudán del Sur", "Suecia", "Suiza", "Surinam", "Sáhara Occidental", "Tailandia", "Tayikistán",
                "Tanzania", "Togo", "Tonga", "Trinidad y Tobago", "Túnez", "Turkmenistán", "Turquía", "Tuvalu",
                "Ucrania", "Uganda", "Uruguay", "Uzbekistán", "Vanuatu", "Vaticano", "Venezuela", "Vietnam", "Yemen",
                "Yibuti", "Zambia", "Zimbabue"
            };
        }

        public static readonly Dictionary<string, string> PaisesPorColor = new Dictionary<string, string>
        {
            { "#6B1100", "TUR" },
            { "#DECD31", "ESP" },
            { "#1C4910", "POR" },
            { "#008404", "ITA" },
            { "#023FE6", "FRA" },
            { "#F7BBBB", "ING" },
            { "#0028BB", "ESC" },
            { "#C3A720", "BEL" },
            { "#DB5E0D", "HOL" },
            { "#EF8F84", "SUI" },
            { "#D92E2E", "AUT" },
            { "#FFF100", "UCR" },
            { "#4F7CD9", "GRE" },
            { "#0097BD", "CRO" },
            { "#C23E15", "MON" },
            { "#9C2C2D", "NOR" },
            { "#ADA210", "SUE" },
            { "#00D4D9", "FIN" },
            { "#D32C00", "DIN" },
            { "#00445A", "CZE" },
            { "#CB7805", "RUM" },
            { "#EB0000", "POL" },
            { "#00003C", "ISR" },
            { "#8337CB", "HUN" },
            { "#E6E5E6", "GEO" },
            { "#8C101C", "ALE" },
        };

        public static string ObtenerCodigoBanderas(string nacionalidad)
        {
            return nacionalidad switch
            {
                "Afganistán" => "AFG",
                "Albania" => "ALB",
                "Alemania" => "DEU",
                "Andorra" => "AND",
                "Angola" => "AGO",
                "Antigua y Barbuda" => "ATG",
                "Arabia Saudita" => "SAU",
                "Argelia" => "DZA",
                "Argentina" => "ARG",
                "Armenia" => "ARM",
                "Australia" => "AUS",
                "Austria" => "AUT",
                "Azerbaiyán" => "AZE",
                "Bahamas" => "BHS",
                "Bangladesh" => "BGD",
                "Barbados" => "BRB",
                "Baréin" => "BHR",
                "Bélgica" => "BEL",
                "Belice" => "BLZ",
                "Benín" => "BEN",
                "Bielorrusia" => "BLR",
                "Birmania" => "MMR",
                "Bolivia" => "BOL",
                "Bosnia y Herzegovina" => "BIH",
                "Botsuana" => "BWA",
                "Brasil" => "BRA",
                "Brunéi" => "BRN",
                "Bulgaria" => "BGR",
                "Burkina Faso" => "BFA",
                "Burundi" => "BDI",
                "Bután" => "BTN",
                "Cabo Verde" => "CPV",
                "Camboya" => "KHM",
                "Camerún" => "CMR",
                "Canadá" => "CAN",
                "Chad" => "TCD",
                "Chile" => "CHL",
                "China" => "CHN",
                "Chipre" => "CYP",
                "Colombia" => "COL",
                "Comoras" => "COM",
                "Congo (República del)" => "COG",
                "Congo (República Democrática del)" => "COD",
                "Corea del Norte" => "PRK",
                "Corea del Sur" => "KOR",
                "Costa Rica" => "CRI",
                "Costa de Marfil" => "CIV",
                "Croacia" => "HRV",
                "Cuba" => "CUB",
                "Curazao" => "CRZ",
                "Dinamarca" => "DNK",
                "Dominica" => "DMA",
                "Ecuador" => "ECU",
                "Egipto" => "EGY",
                "El Salvador" => "SLV",
                "Emiratos Árabes Unidos" => "ARE",
                "Eritrea" => "ERI",
                "Escocia" => "SCO",
                "Eslovaquia" => "SVK",
                "Eslovenia" => "SVN",
                "España" => "ESP",
                "Estados Unidos" => "USA",
                "Estonia" => "EST",
                "Eswatini" => "SWZ",
                "Etiopía" => "ETH",
                "Fiji" => "FJI",
                "Filipinas" => "PHL",
                "Finlandia" => "FIN",
                "Francia" => "FRA",
                "Gabón" => "GAB",
                "Gales" => "WAL",
                "Gambia" => "GMB",
                "Georgia" => "GEO",
                "Ghana" => "GHA",
                "Granada" => "GRD",
                "Grecia" => "GRC",
                "Guatemala" => "GTM",
                "Guinea" => "GIN",
                "Guinea-Bisáu" => "GNB",
                "Guyana" => "GUY",
                "Haití" => "HTI",
                "Honduras" => "HND",
                "Hungría" => "HUN",
                "India" => "IND",
                "Indonesia" => "IDN",
                "Inglaterra" => "GBR",
                "Irak" => "IRQ",
                "Irán" => "IRN",
                "Irlanda" => "IRL",
                "Irlanda del Norte" => "NIR",
                "Islandia" => "ISL",
                "Islas Feroe" => "FER",
                "Islas Marshall" => "MHL",
                "Islas Salomón" => "SLB",
                "Israel" => "ISR",
                "Italia" => "ITA",
                "Jamaica" => "JAM",
                "Japón" => "JPN",
                "Jordania" => "JOR",
                "Kazajistán" => "KAZ",
                "Kenia" => "KEN",
                "Kirguistán" => "KGZ",
                "Kiribati" => "KIR",
                "Kosovo" => "KOS",
                "Kuwait" => "KWT",
                "Laos" => "LAO",
                "Lesoto" => "LSO",
                "Letonia" => "LVA",
                "Líbano" => "LBN",
                "Liberia" => "LBR",
                "Libia" => "LBY",
                "Liechtenstein" => "LIE",
                "Lituania" => "LTU",
                "Luxemburgo" => "LUX",
                "Macedonia del Norte" => "MKD",
                "Madagascar" => "MDG",
                "Malasia" => "MYS",
                "Malawi" => "MWI",
                "Maldivas" => "MDV",
                "Mali" => "MLI",
                "Malta" => "MLT",
                "Moldavia" => "MDA",
                "Marruecos" => "MAR",
                "Martinica" => "MTN",
                "Mauricio" => "MUS",
                "Mauritania" => "MRT",
                "México" => "MEX",
                "Micronesia" => "FSM",
                "Mónaco" => "MCO",
                "Mongolia" => "MNG",
                "Montenegro" => "MNE",
                "Mozambique" => "MOZ",
                "Namibia" => "NAM",
                "Nauru" => "NRU",
                "Nepal" => "NPL",
                "Nicaragua" => "NIC",
                "Níger" => "NER",
                "Nigeria" => "NGA",
                "Noruega" => "NOR",
                "Nueva Zelanda" => "NZL",
                "Omán" => "OMN",
                "Países Bajos" => "NLD",
                "Pakistán" => "PAK",
                "Palaos" => "PLW",
                "Panamá" => "PAN",
                "Papúa Nueva Guinea" => "PNG",
                "Paraguay" => "PRY",
                "Perú" => "PER",
                "Polonia" => "POL",
                "Portugal" => "PRT",
                "Reino Unido" => "GBR",
                "República Checa" => "CZE",
                "República del Congo" => "COG",
                "República Dominicana" => "DOM",
                "Ruanda" => "RWA",
                "Rumanía" => "ROU",
                "Rusia" => "RUS",
                "Samoa" => "WSM",
                "San Cristóbal y Nieves" => "KNA",
                "San Marino" => "SMR",
                "Santo Tomé y Príncipe" => "STP",
                "Senegal" => "SEN",
                "Serbia" => "SRB",
                "Seychelles" => "SYC",
                "Sierra Leona" => "SLE",
                "Singapur" => "SGP",
                "Siria" => "SYR",
                "Somalia" => "SOM",
                "Sri Lanka" => "LKA",
                "Sudáfrica" => "ZAF",
                "Sudán" => "SDN",
                "Sudán del Sur" => "SSD",
                "Suecia" => "SWE",
                "Suiza" => "CHE",
                "Surinam" => "SUR",
                "Sáhara Occidental" => "ESH",
                "Tailandia" => "THA",
                "Tayikistán" => "TJK",
                "Tanzania" => "TZA",
                "Togo" => "TGO",
                "Tonga" => "TON",
                "Trinidad y Tobago" => "TTO",
                "Túnez" => "TUN",
                "Turkmenistán" => "TKM",
                "Turquía" => "TUR",
                "Tuvalu" => "TUV",
                "Ucrania" => "UKR",
                "Uganda" => "UGA",
                "Uruguay" => "URY",
                "Uzbekistán" => "UZB",
                "Vanuatu" => "VUT",
                "Vaticano" => "VAT",
                "Venezuela" => "VEN",
                "Vietnam" => "VNM",
                "Yemen" => "YEM",
                "Yibuti" => "DJI",
                "Zambia" => "ZMB",
                "Zimbabue" => "ZWE",
                _ => "default"
            };
        }

        public static int CambioDivisa(double cantidad)
        {
            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            double cantidadConvertida = cantidad;

            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        cantidadConvertida = cantidadConvertida * Constants.EURO_VALUE;
                        break;
                    case Constants.POUND_NAME:
                        cantidadConvertida = cantidadConvertida * Constants.POUND_VALUE;
                        break;
                    case Constants.DOLLAR_NAME:
                        cantidadConvertida = cantidadConvertida * Constants.DOLLAR_VALUE;
                        break;
                    default:
                        cantidadConvertida = cantidadConvertida * Constants.EURO_VALUE;
                        break;
                }
            }

            return (int)cantidadConvertida;
        }

        public static int CambioDivisaNullable(int? cantidad)
        {
            if (cantidad == null) return 0; // o el valor que quieras como default

            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            double cantidadConvertida = cantidad.Value;

            if (!string.IsNullOrEmpty(currency))
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        cantidadConvertida *= Constants.EURO_VALUE;
                        break;
                    case Constants.POUND_NAME:
                        cantidadConvertida *= Constants.POUND_VALUE;
                        break;
                    case Constants.DOLLAR_NAME:
                        cantidadConvertida *= Constants.DOLLAR_VALUE;
                        break;
                    default:
                        cantidadConvertida *= Constants.EURO_VALUE;
                        break;
                }
            }

            return (int)cantidadConvertida;
        }

        public static string nombreMoneda()
        {
            string currency = PlayerPrefs.GetString("Currency", string.Empty);
            string moneda = EURO_NAME;

            if (currency != string.Empty)
            {
                switch (currency)
                {
                    case Constants.EURO_NAME:
                        moneda = ("Euros");
                        break;
                    case Constants.POUND_NAME:
                        moneda = "Libras esterlinas";
                        break;
                    case Constants.DOLLAR_NAME:
                        moneda = "Dólares americanos";
                        break;
                }
            }

            return moneda;
        }
    }
}

