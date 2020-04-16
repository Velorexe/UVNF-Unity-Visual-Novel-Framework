using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Entities
{
    [Serializable]
    public abstract class Person
    {
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        private string _firstName = "Velo";

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        private string _lastName = "Rex";

        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }
        private string _nickName = "Rexxy";
    }
}