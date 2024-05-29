using System.Text.Json;

namespace WebApplication1.Extensions
{
    public static class SessionExtension
    {
        public static void Save<T>(this ISession session, 
            string name, T data) 
        { 
            var str = JsonSerializer.Serialize(data);
            session.SetString(name, str);
        }

        public static T Read<T>(this ISession session, string name)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            if (name == null) throw new ArgumentNullException();
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException();  
            if(session.GetString(name) == null) return default(T);
            return JsonSerializer.Deserialize<T>(session.GetString(name));
        }
    }
}