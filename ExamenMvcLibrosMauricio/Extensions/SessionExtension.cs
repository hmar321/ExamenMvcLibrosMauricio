using Newtonsoft.Json;

namespace ExamenMvcLibrosMauricio.Extensions
{
    public static class SessionExtension
    {
        public static T GetObject<T>(this ISession session, string key)
        {
            string json = session.GetString(key);
            if (json==null)
            {
                return default(T);
            }
            else
            {
                T objeto = JsonConvert.DeserializeObject<T>(json);
                return objeto;
            }
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            string json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }
    }
}
