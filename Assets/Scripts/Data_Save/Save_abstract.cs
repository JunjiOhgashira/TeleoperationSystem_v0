using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data_Save
{
    abstract class Save_abstract
    {
        private string Sava_name;


        public Save_abstract(string name)
        {
            this.Sava_name = name;
        }


        public abstract string Filename
        {
            set;
            get;
        }


        public abstract void file_open();

        public abstract void file_write();

        public abstract void file_close();
    }
}

