using NexusConnectCRM.Areas.Prospect.Helper;

namespace NexusConnectCRM.Areas.Prospect.Data
{
    public static class States
    {
        public static List<State> GetAll()
        {
            return new List<State>
            {
                new State() { Id = "null", Name = "---" },
                new State() { Id = "AL", Name = "Alabama" },
                new State() { Id = "AK", Name = "Alaska" },
                new State() { Id = "AZ", Name = "Arizona" },
                new State() { Id = "AR", Name = "Arkansas" },
                new State() { Id = "CA", Name = "California" },
                new State() { Id = "CO", Name = "Colorado" },
                new State() { Id = "CT", Name = "Connecticut" },
                new State() { Id = "DE", Name = "Delaware" },
                new State() { Id = "FL", Name = "Florida" },
                new State() { Id = "GA", Name = "Georgia" },
                new State() { Id = "HI", Name = "Hawaii" },
                new State() { Id = "ID", Name = "Idaho" },
                new State() { Id = "IL", Name = "Illinois" },
                new State() { Id = "IN", Name = "Indiana" },
                new State() { Id = "IA", Name = "Iowa" },
                new State() { Id = "KS", Name = "Kansas" },
                new State() { Id = "KY", Name = "Kentucky" },
                new State() { Id = "LA", Name = "Louisiana" },
                new State() { Id = "ME", Name = "Maine" },
                new State() { Id = "MD", Name = "Maryland" },
                new State() { Id = "MA", Name = "Massachusetts" },
                new State() { Id = "MI", Name = "Michigan" },
                new State() { Id = "MN", Name = "Minnesota" },
                new State() { Id = "MS", Name = "Mississippi" },
                new State() { Id = "MO", Name = "Missouri" },
                new State() { Id = "MT", Name = "Montana" },
                new State() { Id = "NE", Name = "Nebraska" },
                new State() { Id = "NV", Name = "Nevada" },
                new State() { Id = "NH", Name = "New Hampshire" },
                new State() { Id = "NJ", Name = "New Jersey" },
                new State() { Id = "NM", Name = "New Mexico" },
                new State() { Id = "NY", Name = "New York" },
                new State() { Id = "NC", Name = "North Carolina" },
                new State() { Id = "ND", Name = "North Dakota" },
                new State() { Id = "OH", Name = "Ohio" },
                new State() { Id = "OK", Name = "Oklahoma" },
                new State() { Id = "OR", Name = "Oregon" },
                new State() { Id = "PA", Name = "Pennsylvania" },
                new State() { Id = "RI", Name = "Rhode Island" },
                new State() { Id = "SC", Name = "South Carolina" },
                new State() { Id = "SD", Name = "South Dakota" },
                new State() { Id = "TN", Name = "Tennessee" },
                new State() { Id = "TX", Name = "Texas" },
                new State() { Id = "UT", Name = "Utah" },
                new State() { Id = "VT", Name = "Vermont" },
                new State() { Id = "VA", Name = "Virginia" },
                new State() { Id = "WA", Name = "Washington" },
                new State() { Id = "WV", Name = "West Virginia" },
                new State() { Id = "WI", Name = "Wisconsin" },
                new State() { Id = "WY", Name = "Wyoming" }
            };
        }
    }
}
