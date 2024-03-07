using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //dbconnect'in save change ve savechangeasync metodları olacak aslında
        Task CommitAsync();
        void Commit(); // asenkron olmayan save change ve savechange olmayan.
    }
}
