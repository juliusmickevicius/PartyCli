namespace partycli.Queries
{
    internal class VpnServerQuery
    {
        public int? Protocol { get; private set; }

        public int? CountryId { get; private set; }

        public int? CityId { get; private set; }

        public int? RegionId { get; private set; }

        public int? SpecificServcerId { get; private set; }

        public int? ServerGroupId { get; private set; }

        public VpnServerQuery(int? protocol, int? countryId, int? cityId, int? regionId, int? specificServcerId, int? serverGroupId)
        {
            Protocol = protocol;
            CountryId = countryId;
            CityId = cityId;
            RegionId = regionId;
            SpecificServcerId = specificServcerId;
            ServerGroupId = serverGroupId;
        }
    }
}
