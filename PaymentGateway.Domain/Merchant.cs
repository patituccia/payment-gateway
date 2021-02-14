using System;

namespace PaymentGateway.Domain
{
    public class Merchant
    {
        public Merchant(int id, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set;  }

        public string Name { get; private set; }
    }
}
