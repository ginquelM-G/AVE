using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlReflect;
using SqlReflectTest.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlReflectTest
{
    public abstract class AbstractRegionDataMapperTest
    {
        protected static readonly string NORTHWIND = @"
                    Server=(LocalDB)\MSSQLLocalDB;
                    Integrated Security=true;
                    AttachDbFileName=" +
                        Environment.CurrentDirectory +
                        "\\data\\NORTHWND.MDF";

        readonly IDataMapper regions;

        public AbstractRegionDataMapperTest(IDataMapper regions)
        {
            this.regions = regions;
        }

        public void TestRegionGetAll()
        {
            IEnumerable res = regions.GetAll();
            int count = 0;
            foreach(object o in res)
            {
                Console.WriteLine(o);
                count++;
            }
            Assert.AreEqual(4, count);

        }


        public void TestRegionGetById()
        {
            Region r = (Region)regions.GetById(1);
            Assert.AreEqual("Eastern                                           ", r.RegionDescription);
            //Assert.AreEqual("Western                                           ", r.RegionDescription); //id=2
        }


        public void TestRegionInsertAndDelete()
        {
            //
            // Create and Insert new Region
            // 
            Region r = new Region()
            {
                RegionID = 5,
                RegionDescription = "Northwest                                         "
            };
            object id = regions.Insert(r);
            //
            // Get the new Region object from database
            //
            Region actual = (Region)regions.GetById(id);
            Assert.AreEqual(r.RegionDescription, actual.RegionDescription);
            //
            // Delete the created Region from database
            //
            regions.Delete(actual);
            object res = regions.GetById(id);
            actual = res != null ? (Region)res : default(Region);
            Assert.IsNull(actual.RegionDescription);
        }

        public void TestRegionUpdate()
        {
            Region original = (Region)regions.GetById(1);
            Region modified = new Region()
            {
                RegionID = original.RegionID,
                RegionDescription = "Eastern2                                          "
               
            };
            regions.Update(modified);
            Region actual = (Region)regions.GetById(1);
            Assert.AreEqual(modified.RegionDescription, actual.RegionDescription);

            regions.Update(original);
            //actual = (Region)regions.GetById(1);
            //Assert.AreEqual("Eastern", actual.RegionDescription);
        }

    }
}
