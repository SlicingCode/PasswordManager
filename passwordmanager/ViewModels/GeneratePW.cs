using passwordmanager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace passwordmanager.ViewModels
{
    public class GeneratePW
    {
        public PasswordGen? CurrentGen { get; set; }
        public GeneratePW() 
        {
            
            CurrentGen = new PasswordGen();
            CurrentGen.Generated = string.Empty;

        }
        
    }
}
