using System.Collections.Generic;

namespace WemaAssessment.Domain.Models
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Local> Locals { get; set; }

        public State()
        {
            Locals = new List<Local>();
        }
    }
}
