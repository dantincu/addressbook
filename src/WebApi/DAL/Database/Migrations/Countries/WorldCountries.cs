﻿using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Migrations.Countries
{
    public static class WorldCountries
    {
        /// <summary>
        /// Gets the complete list of the world's country names.
        /// List taken from <c>https://www.countries-ofthe-world.com/all-countries.html</c>
        /// </summary>
        /// <returns>A list containing all of the world's country names.</returns>
        public static Country[] GetAll() => [
            Country("Afghanistan", "AF"),
            Country("Albania", "AL"),
            Country("Algeria", "DZ"),
            Country("Andorra", "AD"),
            Country("Angola", "AO"),
            Country("Antigua and Barbuda", "AG"),
            Country("Argentina", "AR"),
            Country("Armenia", "AM"),
            Country("Australia", "AU"),
            Country("Austria", "AT"),
            Country("Azerbaijan", "AZ"),

            Country("Bahamas", "BS"),
            Country("Bahrain", "BH"),
            Country("Bangladesh", "BD"),
            Country("Barbados", "BB"),
            Country("Belarus", "BY"),
            Country("Belgium", "BE"),
            Country("Belize", "BZ"),
            Country("Benin", "BJ"),
            Country("Bhutan", "BT"),
            Country("Bolivia", "BO"),
            Country("Bosnia and Herzegovina", "BA"),
            Country("Botswana", "BW"),
            Country("Brazil", "BR"),
            Country("Brunei", "BN"),
            Country("Bulgaria", "BG"),
            Country("Burkina Faso", "BF"),
            Country("Burundi", "BI"),

            Country("Cabo Verde", "CV"),
            Country("Cambodia", "KH"),
            Country("Cameroon", "CM"),
            Country("Canada", "CA"),
            Country("Central African Republic", "CF"),
            Country("Chad", "TD"),
            Country("Chile", "CL"),
            Country("China", "CN"),
            Country("Colombia", "CO"),
            Country("Comoros", "KM"),
            Country("Congo, Democratic Republic of the", "CD"),
            Country("Congo, Republic of the", "CG"),
            Country("Costa Rica", "CR"),
            Country("Cote d'Ivoire", "CI"),
            Country("Croatia", "HR"),
            Country("Cuba", "CU"),
            Country("Cyprus", "CY"),
            Country("Czechia", "CZ"),

            Country("Denmark", "DK"),
            Country("Djibouti", "DJ"),
            Country("Dominica", "DM"),
            Country("Dominican Republic", "DO"),

            Country("Ecuador", "EC"),
            Country("Egypt", "EG"),
            Country("El Salvador", "SV"),
            Country("Equatorial Guinea", "GQ"),
            Country("Eritrea", "ER"),
            Country("Estonia", "EE"),
            Country("Eswatini", "SZ"),
            Country("Ethiopia", "ET"),

            Country("Fiji", "FJ"),
            Country("Finland", "FI"),
            Country("France", "FR"),

            Country("Gabon", "GA"),
            Country("Gambia", "GM"),
            Country("Georgia", "GE"),
            Country("Germany", "DE"),
            Country("Ghana", "GH"),
            Country("Greece", "GR"),
            Country("Grenada", "GD"),
            Country("Guatemala", "GT"),
            Country("Guinea", "GN"),
            Country("Guinea-Bissau", "GW"),
            Country("Guyana", "GY"),

            Country("Haiti", "HT"),
            Country("Honduras", "HN"),
            Country("Hungary", "HU"),

            Country("Iceland", "IS"),
            Country("India", "IN"),
            Country("Indonesia", "ID"),
            Country("Iran", "IR"),
            Country("Iraq", "IQ"),
            Country("Ireland", "IE"),
            Country("Israel", "IE"),
            Country("Italy", "IT"),

            Country("Jamaica", "JM"),
            Country("Japan", "JP"),
            Country("Jordan", "JO"),

            Country("Kazakhstan", "KZ"),
            Country("Kenya", "KE"),
            Country("Kiribati", "KI"),
            Country("Kosovo", "XK"),
            Country("Kuwait", "KW"),
            Country("Kyrgyzstan", "KG"),

            Country("Laos", "LA"),
            Country("Latvia", "LV"),
            Country("Lebanon", "LB"),
            Country("Lesotho", "LS"),
            Country("Liberia", "LR"),
            Country("Libya", "LY"),
            Country("Liechtenstein", "LI"),
            Country("Lithuania", "LT"),
            Country("Luxembourg", "LU"),

            Country("Madagascar", "MG"),
            Country("Malawi", "MW"),
            Country("Malaysia", "MY"),
            Country("Maldives", "MV"),
            Country("Mali", "ML"),
            Country("Malta", "MT"),
            Country("Marshall Islands", "MH"),
            Country("Mauritania", "MR"),
            Country("Mauritius", "MU"),
            Country("Mexico", "MX"),
            Country("Micronesia", "FM"),
            Country("Moldova", "MD"),
            Country("Monaco", "MC"),
            Country("Mongolia", "MN"),
            Country("Montenegro", "ME"),
            Country("Morocco", "MA"),
            Country("Mozambique", "MZ"),
            Country("Myanmar", "MM"),

            Country("Namibia", "NA"),
            Country("Nauru", "NR"),
            Country("Nepal", "NP"),
            Country("Netherlands", "NL"),
            Country("New Zealand", "NZ"),
            Country("Nicaragua", "NI"),
            Country("Niger", "NE"),
            Country("Nigeria", "NG"),
            Country("North Korea", "KP"),
            Country("North Macedonia", "MK"),
            Country("Norway", "NO"),

            Country("Oman", "OM"),

            Country("Pakistan", "PK"),
            Country("Palau", "PW"),
            Country("Palestine", "PS"),
            Country("Panama", "PA"),
            Country("Papua New Guinea", "PG"),
            Country("Paraguay", "PY"),
            Country("Peru", "PE"),
            Country("Philippines", "PH"),
            Country("Poland", "PL"),
            Country("Portugal", "PT"),

            Country("Qatar", "QA"),

            Country("Romania", "RO"),
            Country("Russia", "RU"),
            Country("Rwanda", "RW"),

            Country("Saint Kitts and Nevis", "KN"),
            Country("Saint Lucia", "LC"),
            Country("Saint Vincent and the Grenadines", "VC"),
            Country("Samoa", "WS"),
            Country("San Marino", "SM"),
            Country("Sao Tome and Principe", "ST"),
            Country("Saudi Arabia", "SA"),
            Country("Senegal", "SN"),
            Country("Serbia", "RS"),
            Country("Seychelles", "SC"),
            Country("Sierra Leone", "SL"),
            Country("Singapore", "SG"),
            Country("Slovakia", "SK"),
            Country("Slovenia", "SI"),
            Country("Solomon Islands", "SB"),
            Country("Somalia", "SO"),
            Country("South Africa", "ZA"),
            Country("South Korea", "KR"),
            Country("South Sudan", "SS"),
            Country("Spain", "ES"),
            Country("Sri Lanka", "LK"),
            Country("Sudan", "SD"),
            Country("Suriname", "SR"),
            Country("Sweden", "SE"),
            Country("Switzerland", "CH"),
            Country("Syria", "SY"),

            Country("Taiwan", "TW"),
            Country("Tajikistan", "TJ"),
            Country("Tanzania", "TZ"),
            Country("Thailand", "TH"),
            Country("Timor-Leste", "TL"),
            Country("Togo", "TG"),
            Country("Tonga", "TO"),
            Country("Trinidad and Tobago", "TT"),
            Country("Tunisia", "TN"),
            Country("Turkey", "TR"),
            Country("Turkmenistan", "TM"),
            Country("Tuvalu", "TV"),

            Country("Uganda", "UG"),
            Country("Ukraine", "UA"),
            Country("United Arab Emirates (UAE)", "AE"),
            Country("United Kingdom (UK)", "GB"),
            Country("United States of America (USA)", "US", [
                "Alabama",
                "Alaska",
                "Arizona",
                "Arkansas",
                "California",
                "Colorado",
                "Connecticut",
                "Delaware",
                "Florida",
                "Georgia",
                "Hawaii",
                "Idaho",
                "Illinois",
                "Indiana",
                "Iowa",
                "Kansas",
                "Kentucky",
                "Louisiana",
                "Maine",
                "Maryland",
                "Massachusetts",
                "Michigan",
                "Minnesota",
                "Mississippi",
                "Missouri",
                "Montana",
                "Nebraska",
                "Nevada",
                "New Hampshire",
                "New Jersey",
                "New Mexico",
                "New York",
                "North Carolina",
                "North Dakota",
                "Ohio",
                "Oklahoma",
                "Oregon",
                "Pennsylvania",
                "Rhode Island",
                "South Carolina",
                "South Dakota",
                "Tennessee",
                "Texas",
                "Utah",
                "Vermont",
                "Virginia",
                "Washington",
                "West Virginia",
                "Wisconsin",
                "Wyoming"]),
            Country("Uruguay", "UY"),
            Country("Uzbekistan", "UZ"),

            Country("Vanuatu", "VU"),
            Country("Vatican City (Holy See)", "VA"),
            Country("Venezuela", "VE"),
            Country("Vietnam", "VN"),

            Country("Yemen", "YE"),

            Country("Zambia", "ZM"),
            Country("Zimbabwe", "ZW")];

    private static Country Country(
            string name,
            string code,
            string[]? countyNames = null) => CountryCore(
                name, code, countyNames?.Select(
                    name => new County
                    {
                        Name = name
                    }).ToList());

    private static Country CountryCore(
            string name,
            string code,
            List<County>? counties = null) => new Country
            {
                Name = name,
                Code = code,
                Counties = counties!,
            };
    }
}
