using System;
using System.Text;
using Microsoft.Isam.Esent.Interop;

namespace Kiwi.Fluesent
{
    public static class SyntaxForTableKey
    {
        public static ITableKey Int64(this ITableKey key, Int64 value)
        {
            key.DefineKeyPart((s, t, m) => Api.MakeKey(s, t, value, m & MakeKeyGrbit.NewKey));
            return key;
        }

        public static ITableKey String(this ITableKey key, string value)
        {
            return String(key, value, Encoding.Unicode);
        }

        public static ITableKey String(this ITableKey key, string value, Encoding encoding)
        {
            key.DefineKeyPart((s,t,m) => Api.MakeKey(s,t,value,encoding, m & MakeKeyGrbit.NewKey));
            return key;
        }
    }
}