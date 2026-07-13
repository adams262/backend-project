using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaborStats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeoSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "geo",
                table: "voivodeships",
                columns: new[] { "teryt", "name" },
                values: new object[,]
                {
                    { "02", "DOLNOŚLĄSKIE" },
                    { "04", "KUJAWSKO-POMORSKIE" },
                    { "06", "LUBELSKIE" },
                    { "08", "LUBUSKIE" },
                    { "10", "ŁÓDZKIE" },
                    { "12", "MAŁOPOLSKIE" },
                    { "14", "MAZOWIECKIE" },
                    { "16", "OPOLSKIE" },
                    { "18", "PODKARPACKIE" },
                    { "20", "PODLASKIE" },
                    { "22", "POMORSKIE" },
                    { "24", "ŚLĄSKIE" },
                    { "26", "ŚWIĘTOKRZYSKIE" },
                    { "28", "WARMIŃSKO-MAZURSKIE" },
                    { "30", "WIELKOPOLSKIE" },
                    { "32", "ZACHODNIOPOMORSKIE" }
                });

            migrationBuilder.InsertData(
                schema: "geo",
                table: "counties",
                columns: new[] { "teryt", "is_city_with_county_rights", "name", "voivodeship_teryt" },
                values: new object[,]
                {
                    { "0201", false, "bolesławiecki", "02" },
                    { "0202", false, "dzierżoniowski", "02" },
                    { "0203", false, "głogowski", "02" },
                    { "0204", false, "górowski", "02" },
                    { "0205", false, "jaworski", "02" },
                    { "0206", false, "karkonoski", "02" },
                    { "0207", false, "kamiennogórski", "02" },
                    { "0208", false, "kłodzki", "02" },
                    { "0209", false, "legnicki", "02" },
                    { "0210", false, "lubański", "02" },
                    { "0211", false, "lubiński", "02" },
                    { "0212", false, "lwówecki", "02" },
                    { "0213", false, "milicki", "02" },
                    { "0214", false, "oleśnicki", "02" },
                    { "0215", false, "oławski", "02" },
                    { "0216", false, "polkowicki", "02" },
                    { "0217", false, "strzeliński", "02" },
                    { "0218", false, "średzki", "02" },
                    { "0219", false, "świdnicki", "02" },
                    { "0220", false, "trzebnicki", "02" },
                    { "0221", false, "wałbrzyski", "02" },
                    { "0222", false, "wołowski", "02" },
                    { "0223", false, "wrocławski", "02" },
                    { "0224", false, "ząbkowicki", "02" },
                    { "0225", false, "zgorzelecki", "02" },
                    { "0226", false, "złotoryjski", "02" },
                    { "0261", true, "Jelenia Góra", "02" },
                    { "0262", true, "Legnica", "02" },
                    { "0264", true, "Wrocław", "02" },
                    { "0265", true, "Wałbrzych", "02" },
                    { "0401", false, "aleksandrowski", "04" },
                    { "0402", false, "brodnicki", "04" },
                    { "0403", false, "bydgoski", "04" },
                    { "0404", false, "chełmiński", "04" },
                    { "0405", false, "golubsko-dobrzyński", "04" },
                    { "0406", false, "grudziądzki", "04" },
                    { "0407", false, "inowrocławski", "04" },
                    { "0408", false, "lipnowski", "04" },
                    { "0409", false, "mogileński", "04" },
                    { "0410", false, "nakielski", "04" },
                    { "0411", false, "radziejowski", "04" },
                    { "0412", false, "rypiński", "04" },
                    { "0413", false, "sępoleński", "04" },
                    { "0414", false, "świecki", "04" },
                    { "0415", false, "toruński", "04" },
                    { "0416", false, "tucholski", "04" },
                    { "0417", false, "wąbrzeski", "04" },
                    { "0418", false, "włocławski", "04" },
                    { "0419", false, "żniński", "04" },
                    { "0461", true, "Bydgoszcz", "04" },
                    { "0462", true, "Grudziądz", "04" },
                    { "0463", true, "Toruń", "04" },
                    { "0464", true, "Włocławek", "04" },
                    { "0601", false, "bialski", "06" },
                    { "0602", false, "biłgorajski", "06" },
                    { "0603", false, "chełmski", "06" },
                    { "0604", false, "hrubieszowski", "06" },
                    { "0605", false, "janowski", "06" },
                    { "0606", false, "krasnostawski", "06" },
                    { "0607", false, "kraśnicki", "06" },
                    { "0608", false, "lubartowski", "06" },
                    { "0609", false, "lubelski", "06" },
                    { "0610", false, "łęczyński", "06" },
                    { "0611", false, "łukowski", "06" },
                    { "0612", false, "opolski", "06" },
                    { "0613", false, "parczewski", "06" },
                    { "0614", false, "puławski", "06" },
                    { "0615", false, "radzyński", "06" },
                    { "0616", false, "rycki", "06" },
                    { "0617", false, "świdnicki", "06" },
                    { "0618", false, "tomaszowski", "06" },
                    { "0619", false, "włodawski", "06" },
                    { "0620", false, "zamojski", "06" },
                    { "0661", true, "Biała Podlaska", "06" },
                    { "0662", true, "Chełm", "06" },
                    { "0663", true, "Lublin", "06" },
                    { "0664", true, "Zamość", "06" },
                    { "0801", false, "gorzowski", "08" },
                    { "0802", false, "krośnieński", "08" },
                    { "0803", false, "międzyrzecki", "08" },
                    { "0804", false, "nowosolski", "08" },
                    { "0805", false, "słubicki", "08" },
                    { "0806", false, "strzelecko-drezdenecki", "08" },
                    { "0807", false, "sulęciński", "08" },
                    { "0808", false, "świebodziński", "08" },
                    { "0809", false, "zielonogórski", "08" },
                    { "0810", false, "żagański", "08" },
                    { "0811", false, "żarski", "08" },
                    { "0812", false, "wschowski", "08" },
                    { "0861", true, "Gorzów Wielkopolski", "08" },
                    { "0862", true, "Zielona Góra", "08" },
                    { "1001", false, "bełchatowski", "10" },
                    { "1002", false, "kutnowski", "10" },
                    { "1003", false, "łaski", "10" },
                    { "1004", false, "łęczycki", "10" },
                    { "1005", false, "łowicki", "10" },
                    { "1006", false, "łódzki wschodni", "10" },
                    { "1007", false, "opoczyński", "10" },
                    { "1008", false, "pabianicki", "10" },
                    { "1009", false, "pajęczański", "10" },
                    { "1010", false, "piotrkowski", "10" },
                    { "1011", false, "poddębicki", "10" },
                    { "1012", false, "radomszczański", "10" },
                    { "1013", false, "rawski", "10" },
                    { "1014", false, "sieradzki", "10" },
                    { "1015", false, "skierniewicki", "10" },
                    { "1016", false, "tomaszowski", "10" },
                    { "1017", false, "wieluński", "10" },
                    { "1018", false, "wieruszowski", "10" },
                    { "1019", false, "zduńskowolski", "10" },
                    { "1020", false, "zgierski", "10" },
                    { "1021", false, "brzeziński", "10" },
                    { "1061", true, "Łódź", "10" },
                    { "1062", true, "Piotrków Trybunalski", "10" },
                    { "1063", true, "Skierniewice", "10" },
                    { "1201", false, "bocheński", "12" },
                    { "1202", false, "brzeski", "12" },
                    { "1203", false, "chrzanowski", "12" },
                    { "1204", false, "dąbrowski", "12" },
                    { "1205", false, "gorlicki", "12" },
                    { "1206", false, "krakowski", "12" },
                    { "1207", false, "limanowski", "12" },
                    { "1208", false, "miechowski", "12" },
                    { "1209", false, "myślenicki", "12" },
                    { "1210", false, "nowosądecki", "12" },
                    { "1211", false, "nowotarski", "12" },
                    { "1212", false, "olkuski", "12" },
                    { "1213", false, "oświęcimski", "12" },
                    { "1214", false, "proszowicki", "12" },
                    { "1215", false, "suski", "12" },
                    { "1216", false, "tarnowski", "12" },
                    { "1217", false, "tatrzański", "12" },
                    { "1218", false, "wadowicki", "12" },
                    { "1219", false, "wielicki", "12" },
                    { "1261", true, "Kraków", "12" },
                    { "1262", true, "Nowy Sącz", "12" },
                    { "1263", true, "Tarnów", "12" },
                    { "1401", false, "białobrzeski", "14" },
                    { "1402", false, "ciechanowski", "14" },
                    { "1403", false, "garwoliński", "14" },
                    { "1404", false, "gostyniński", "14" },
                    { "1405", false, "grodziski", "14" },
                    { "1406", false, "grójecki", "14" },
                    { "1407", false, "kozienicki", "14" },
                    { "1408", false, "legionowski", "14" },
                    { "1409", false, "lipski", "14" },
                    { "1410", false, "łosicki", "14" },
                    { "1411", false, "makowski", "14" },
                    { "1412", false, "miński", "14" },
                    { "1413", false, "mławski", "14" },
                    { "1414", false, "nowodworski", "14" },
                    { "1415", false, "ostrołęcki", "14" },
                    { "1416", false, "ostrowski", "14" },
                    { "1417", false, "otwocki", "14" },
                    { "1418", false, "piaseczyński", "14" },
                    { "1419", false, "płocki", "14" },
                    { "1420", false, "płoński", "14" },
                    { "1421", false, "pruszkowski", "14" },
                    { "1422", false, "przasnyski", "14" },
                    { "1423", false, "przysuski", "14" },
                    { "1424", false, "pułtuski", "14" },
                    { "1425", false, "radomski", "14" },
                    { "1426", false, "siedlecki", "14" },
                    { "1427", false, "sierpecki", "14" },
                    { "1428", false, "sochaczewski", "14" },
                    { "1429", false, "sokołowski", "14" },
                    { "1430", false, "szydłowiecki", "14" },
                    { "1432", false, "warszawski zachodni", "14" },
                    { "1433", false, "węgrowski", "14" },
                    { "1434", false, "wołomiński", "14" },
                    { "1435", false, "wyszkowski", "14" },
                    { "1436", false, "zwoleński", "14" },
                    { "1437", false, "żuromiński", "14" },
                    { "1438", false, "żyrardowski", "14" },
                    { "1461", true, "Ostrołęka", "14" },
                    { "1462", true, "Płock", "14" },
                    { "1463", true, "Radom", "14" },
                    { "1464", true, "Siedlce", "14" },
                    { "1465", true, "Warszawa", "14" },
                    { "1601", false, "brzeski", "16" },
                    { "1602", false, "głubczycki", "16" },
                    { "1603", false, "kędzierzyńsko-kozielski", "16" },
                    { "1604", false, "kluczborski", "16" },
                    { "1605", false, "krapkowicki", "16" },
                    { "1606", false, "namysłowski", "16" },
                    { "1607", false, "nyski", "16" },
                    { "1608", false, "oleski", "16" },
                    { "1609", false, "opolski", "16" },
                    { "1610", false, "prudnicki", "16" },
                    { "1611", false, "strzelecki", "16" },
                    { "1661", true, "Opole", "16" },
                    { "1801", false, "bieszczadzki", "18" },
                    { "1802", false, "brzozowski", "18" },
                    { "1803", false, "dębicki", "18" },
                    { "1804", false, "jarosławski", "18" },
                    { "1805", false, "jasielski", "18" },
                    { "1806", false, "kolbuszowski", "18" },
                    { "1807", false, "krośnieński", "18" },
                    { "1808", false, "leżajski", "18" },
                    { "1809", false, "lubaczowski", "18" },
                    { "1810", false, "łańcucki", "18" },
                    { "1811", false, "mielecki", "18" },
                    { "1812", false, "niżański", "18" },
                    { "1813", false, "przemyski", "18" },
                    { "1814", false, "przeworski", "18" },
                    { "1815", false, "ropczycko-sędziszowski", "18" },
                    { "1816", false, "rzeszowski", "18" },
                    { "1817", false, "sanocki", "18" },
                    { "1818", false, "stalowowolski", "18" },
                    { "1819", false, "strzyżowski", "18" },
                    { "1820", false, "tarnobrzeski", "18" },
                    { "1821", false, "leski", "18" },
                    { "1861", true, "Krosno", "18" },
                    { "1862", true, "Przemyśl", "18" },
                    { "1863", true, "Rzeszów", "18" },
                    { "1864", true, "Tarnobrzeg", "18" },
                    { "2001", false, "augustowski", "20" },
                    { "2002", false, "białostocki", "20" },
                    { "2003", false, "bielski", "20" },
                    { "2004", false, "grajewski", "20" },
                    { "2005", false, "hajnowski", "20" },
                    { "2006", false, "kolneński", "20" },
                    { "2007", false, "łomżyński", "20" },
                    { "2008", false, "moniecki", "20" },
                    { "2009", false, "sejneński", "20" },
                    { "2010", false, "siemiatycki", "20" },
                    { "2011", false, "sokólski", "20" },
                    { "2012", false, "suwalski", "20" },
                    { "2013", false, "wysokomazowiecki", "20" },
                    { "2014", false, "zambrowski", "20" },
                    { "2061", true, "Białystok", "20" },
                    { "2062", true, "Łomża", "20" },
                    { "2063", true, "Suwałki", "20" },
                    { "2201", false, "bytowski", "22" },
                    { "2202", false, "chojnicki", "22" },
                    { "2203", false, "człuchowski", "22" },
                    { "2204", false, "gdański", "22" },
                    { "2205", false, "kartuski", "22" },
                    { "2206", false, "kościerski", "22" },
                    { "2207", false, "kwidzyński", "22" },
                    { "2208", false, "lęborski", "22" },
                    { "2209", false, "malborski", "22" },
                    { "2210", false, "nowodworski", "22" },
                    { "2211", false, "pucki", "22" },
                    { "2212", false, "słupski", "22" },
                    { "2213", false, "starogardzki", "22" },
                    { "2214", false, "tczewski", "22" },
                    { "2215", false, "wejherowski", "22" },
                    { "2216", false, "sztumski", "22" },
                    { "2261", true, "Gdańsk", "22" },
                    { "2262", true, "Gdynia", "22" },
                    { "2263", true, "Słupsk", "22" },
                    { "2264", true, "Sopot", "22" },
                    { "2401", false, "będziński", "24" },
                    { "2402", false, "bielski", "24" },
                    { "2403", false, "cieszyński", "24" },
                    { "2404", false, "częstochowski", "24" },
                    { "2405", false, "gliwicki", "24" },
                    { "2406", false, "kłobucki", "24" },
                    { "2407", false, "lubliniecki", "24" },
                    { "2408", false, "mikołowski", "24" },
                    { "2409", false, "myszkowski", "24" },
                    { "2410", false, "pszczyński", "24" },
                    { "2411", false, "raciborski", "24" },
                    { "2412", false, "rybnicki", "24" },
                    { "2413", false, "tarnogórski", "24" },
                    { "2414", false, "bieruńsko-lędziński", "24" },
                    { "2415", false, "wodzisławski", "24" },
                    { "2416", false, "zawierciański", "24" },
                    { "2417", false, "żywiecki", "24" },
                    { "2461", true, "Bielsko-Biała", "24" },
                    { "2462", true, "Bytom", "24" },
                    { "2463", true, "Chorzów", "24" },
                    { "2464", true, "Częstochowa", "24" },
                    { "2465", true, "Dąbrowa Górnicza", "24" },
                    { "2466", true, "Gliwice", "24" },
                    { "2467", true, "Jastrzębie-Zdrój", "24" },
                    { "2468", true, "Jaworzno", "24" },
                    { "2469", true, "Katowice", "24" },
                    { "2470", true, "Mysłowice", "24" },
                    { "2471", true, "Piekary Śląskie", "24" },
                    { "2472", true, "Ruda Śląska", "24" },
                    { "2473", true, "Rybnik", "24" },
                    { "2474", true, "Siemianowice Śląskie", "24" },
                    { "2475", true, "Sosnowiec", "24" },
                    { "2476", true, "Świętochłowice", "24" },
                    { "2477", true, "Tychy", "24" },
                    { "2478", true, "Zabrze", "24" },
                    { "2479", true, "Żory", "24" },
                    { "2601", false, "buski", "26" },
                    { "2602", false, "jędrzejowski", "26" },
                    { "2603", false, "kazimierski", "26" },
                    { "2604", false, "kielecki", "26" },
                    { "2605", false, "konecki", "26" },
                    { "2606", false, "opatowski", "26" },
                    { "2607", false, "ostrowiecki", "26" },
                    { "2608", false, "pińczowski", "26" },
                    { "2609", false, "sandomierski", "26" },
                    { "2610", false, "skarżyski", "26" },
                    { "2611", false, "starachowicki", "26" },
                    { "2612", false, "staszowski", "26" },
                    { "2613", false, "włoszczowski", "26" },
                    { "2661", true, "Kielce", "26" },
                    { "2801", false, "bartoszycki", "28" },
                    { "2802", false, "braniewski", "28" },
                    { "2803", false, "działdowski", "28" },
                    { "2804", false, "elbląski", "28" },
                    { "2805", false, "ełcki", "28" },
                    { "2806", false, "giżycki", "28" },
                    { "2807", false, "iławski", "28" },
                    { "2808", false, "kętrzyński", "28" },
                    { "2809", false, "lidzbarski", "28" },
                    { "2810", false, "mrągowski", "28" },
                    { "2811", false, "nidzicki", "28" },
                    { "2812", false, "nowomiejski", "28" },
                    { "2813", false, "olecki", "28" },
                    { "2814", false, "olsztyński", "28" },
                    { "2815", false, "ostródzki", "28" },
                    { "2816", false, "piski", "28" },
                    { "2817", false, "szczycieński", "28" },
                    { "2818", false, "gołdapski", "28" },
                    { "2819", false, "węgorzewski", "28" },
                    { "2861", true, "Elbląg", "28" },
                    { "2862", true, "Olsztyn", "28" },
                    { "3001", false, "chodzieski", "30" },
                    { "3002", false, "czarnkowsko-trzcianecki", "30" },
                    { "3003", false, "gnieźnieński", "30" },
                    { "3004", false, "gostyński", "30" },
                    { "3005", false, "grodziski", "30" },
                    { "3006", false, "jarociński", "30" },
                    { "3007", false, "kaliski", "30" },
                    { "3008", false, "kępiński", "30" },
                    { "3009", false, "kolski", "30" },
                    { "3010", false, "koniński", "30" },
                    { "3011", false, "kościański", "30" },
                    { "3012", false, "krotoszyński", "30" },
                    { "3013", false, "leszczyński", "30" },
                    { "3014", false, "międzychodzki", "30" },
                    { "3015", false, "nowotomyski", "30" },
                    { "3016", false, "obornicki", "30" },
                    { "3017", false, "ostrowski", "30" },
                    { "3018", false, "ostrzeszowski", "30" },
                    { "3019", false, "pilski", "30" },
                    { "3020", false, "pleszewski", "30" },
                    { "3021", false, "poznański", "30" },
                    { "3022", false, "rawicki", "30" },
                    { "3023", false, "słupecki", "30" },
                    { "3024", false, "szamotulski", "30" },
                    { "3025", false, "średzki", "30" },
                    { "3026", false, "śremski", "30" },
                    { "3027", false, "turecki", "30" },
                    { "3028", false, "wągrowiecki", "30" },
                    { "3029", false, "wolsztyński", "30" },
                    { "3030", false, "wrzesiński", "30" },
                    { "3031", false, "złotowski", "30" },
                    { "3061", true, "Kalisz", "30" },
                    { "3062", true, "Konin", "30" },
                    { "3063", true, "Leszno", "30" },
                    { "3064", true, "Poznań", "30" },
                    { "3201", false, "białogardzki", "32" },
                    { "3202", false, "choszczeński", "32" },
                    { "3203", false, "drawski", "32" },
                    { "3204", false, "goleniowski", "32" },
                    { "3205", false, "gryficki", "32" },
                    { "3206", false, "gryfiński", "32" },
                    { "3207", false, "kamieński", "32" },
                    { "3208", false, "kołobrzeski", "32" },
                    { "3209", false, "koszaliński", "32" },
                    { "3210", false, "myśliborski", "32" },
                    { "3211", false, "policki", "32" },
                    { "3212", false, "pyrzycki", "32" },
                    { "3213", false, "sławieński", "32" },
                    { "3214", false, "stargardzki", "32" },
                    { "3215", false, "szczecinecki", "32" },
                    { "3216", false, "świdwiński", "32" },
                    { "3217", false, "wałecki", "32" },
                    { "3218", false, "łobeski", "32" },
                    { "3261", true, "Koszalin", "32" },
                    { "3262", true, "Szczecin", "32" },
                    { "3263", true, "Świnoujście", "32" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0201");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0202");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0203");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0204");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0205");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0206");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0207");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0208");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0209");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0210");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0211");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0212");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0213");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0214");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0215");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0216");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0217");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0218");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0219");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0220");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0221");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0222");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0223");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0224");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0225");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0226");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0261");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0262");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0264");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0265");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0401");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0402");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0403");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0404");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0405");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0406");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0407");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0408");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0409");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0410");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0411");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0412");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0413");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0414");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0415");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0416");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0417");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0418");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0419");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0461");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0462");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0463");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0464");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0601");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0602");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0603");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0604");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0605");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0606");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0607");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0608");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0609");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0610");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0611");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0612");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0613");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0614");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0615");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0616");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0617");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0618");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0619");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0620");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0661");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0662");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0663");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0664");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0801");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0802");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0803");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0804");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0805");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0806");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0807");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0808");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0809");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0810");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0811");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0812");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0861");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "0862");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1001");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1002");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1003");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1004");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1005");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1006");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1007");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1008");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1009");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1010");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1011");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1012");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1013");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1014");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1015");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1016");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1017");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1018");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1019");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1020");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1021");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1061");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1062");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1063");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1201");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1202");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1203");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1204");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1205");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1206");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1207");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1208");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1209");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1210");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1211");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1212");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1213");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1214");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1215");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1216");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1217");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1218");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1219");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1261");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1262");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1263");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1401");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1402");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1403");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1404");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1405");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1406");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1407");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1408");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1409");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1410");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1411");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1412");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1413");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1414");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1415");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1416");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1417");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1418");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1419");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1420");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1421");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1422");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1423");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1424");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1425");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1426");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1427");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1428");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1429");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1430");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1432");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1433");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1434");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1435");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1436");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1437");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1438");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1461");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1462");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1463");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1464");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1465");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1601");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1602");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1603");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1604");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1605");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1606");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1607");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1608");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1609");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1610");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1611");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1661");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1801");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1802");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1803");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1804");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1805");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1806");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1807");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1808");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1809");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1810");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1811");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1812");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1813");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1814");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1815");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1816");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1817");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1818");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1819");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1820");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1821");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1861");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1862");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1863");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "1864");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2001");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2002");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2003");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2004");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2005");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2006");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2007");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2008");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2009");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2010");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2011");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2012");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2013");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2014");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2061");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2062");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2063");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2201");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2202");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2203");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2204");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2205");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2206");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2207");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2208");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2209");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2210");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2211");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2212");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2213");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2214");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2215");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2216");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2261");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2262");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2263");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2264");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2401");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2402");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2403");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2404");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2405");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2406");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2407");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2408");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2409");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2410");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2411");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2412");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2413");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2414");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2415");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2416");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2417");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2461");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2462");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2463");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2464");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2465");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2466");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2467");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2468");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2469");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2470");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2471");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2472");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2473");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2474");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2475");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2476");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2477");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2478");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2479");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2601");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2602");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2603");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2604");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2605");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2606");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2607");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2608");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2609");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2610");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2611");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2612");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2613");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2661");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2801");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2802");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2803");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2804");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2805");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2806");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2807");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2808");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2809");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2810");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2811");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2812");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2813");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2814");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2815");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2816");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2817");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2818");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2819");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2861");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "2862");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3001");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3002");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3003");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3004");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3005");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3006");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3007");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3008");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3009");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3010");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3011");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3012");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3013");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3014");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3015");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3016");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3017");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3018");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3019");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3020");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3021");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3022");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3023");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3024");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3025");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3026");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3027");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3028");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3029");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3030");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3031");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3061");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3062");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3063");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3064");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3201");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3202");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3203");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3204");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3205");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3206");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3207");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3208");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3209");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3210");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3211");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3212");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3213");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3214");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3215");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3216");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3217");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3218");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3261");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3262");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "counties",
                keyColumn: "teryt",
                keyValue: "3263");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "02");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "04");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "06");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "08");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "10");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "12");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "14");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "16");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "18");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "20");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "22");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "24");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "26");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "28");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "30");

            migrationBuilder.DeleteData(
                schema: "geo",
                table: "voivodeships",
                keyColumn: "teryt",
                keyValue: "32");
        }
    }
}
