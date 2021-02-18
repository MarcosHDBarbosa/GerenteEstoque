using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  internal class PrimaryKey : Field
  {
    public PrimaryKey(string name, GetValue getValue = null, SetValue setValue = null) : base(name, getValue, setValue)
    {
    }
  }
}
