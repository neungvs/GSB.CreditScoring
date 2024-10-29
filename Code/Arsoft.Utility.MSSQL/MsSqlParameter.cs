using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Arsoft.Utility.MSSQL
{
    public class MsSqlParameter
    {
        #region "Attributes"
            string _name;
            object _value;
            bool _direction;
            SqlDbType _type;
            int _size;
            int _precision = 0;
            int _scale = 0;
        #endregion

        #region "Methods"

        public MsSqlParameter()
        {
        }

        public MsSqlParameter(string name, object value)
        {
            _name = name;
            _value = value;
        }

        public MsSqlParameter(string name, object value,bool direction=false)
        {
            _name = name;
            _value = value;
            _direction = direction;
        }

        public MsSqlParameter(string name, SqlDbType type, int size, int precision = 0, int scale = 0)
        {
            _name = name;
            _type = type;
            _size = size;
            _precision = precision;
            _scale = scale;
        }

        public MsSqlParameter(string name, object value, SqlDbType type, int size, int precision = 0, int scale = 0)
        {
            _name = name;
            _value = value;
            _type = type;
            _size = size;
            _precision = precision;
            _scale = scale;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Values
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Direction
        {
            get { return _direction; }
            set { _direction=value; }
        }

        public SqlDbType DbType
        {
            get { return _type; }
            set { _type = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int Precision
        {
            get { return _precision; }
            set { _precision = value; }
        }

        public int Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        #endregion
    }
}
