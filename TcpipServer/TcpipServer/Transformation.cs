using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace mng
{
    public static class Transformation
    {
        #region преобразование из даты в байты (сериализация)
        public static byte[] stringToByteArray(string stringToConvert)
        {
            var encoding = new UnicodeEncoding();
            return encoding.GetBytes(stringToConvert);
        }

        public static byte[] convertDataTableToByteArray(DataTable dataTable)
        {
            byte[] binaryDataResult;
            var brFormatter = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

        public static byte[] convertDataSetToByteArray(DataSet dataSet)
        {
            byte[] binaryDataResult;
            using (var memStream = new MemoryStream())
            {
                var brFormatter = new BinaryFormatter();
                dataSet.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataSet);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }
        #endregion

        #region преобразование из байтов в дату (десериализация)
        public static DataTable convertByteArrayToDataTable(byte[] byteDataArray)
        {
            DataTable dataTable;
            var brFormatter = new BinaryFormatter();
            using (var memStream = new MemoryStream(byteDataArray))
            {
                dataTable = (DataTable)brFormatter.Deserialize(memStream);
                memStream.Close();
            }
            return dataTable;
        }

        public static DataSet convertByteArrayToDataSet(byte[] byteDataArray)
        {
            DataSet dataSet;
            var brFormatter = new BinaryFormatter();
            using (var memStream = new MemoryStream(byteDataArray))
            {
                dataSet = (DataSet)brFormatter.Deserialize(memStream);
                memStream.Close();
            }
            return dataSet;
        }
        #endregion
    }
}
