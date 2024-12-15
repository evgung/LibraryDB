using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDB;

public static class NpgsqlTypeMatcher
{
    private static readonly Dictionary<string, NpgsqlDbType> typeMatching = new()
    {
        { "integer", NpgsqlDbType.Integer },
        { "bigint", NpgsqlDbType.Bigint },
        { "smallint", NpgsqlDbType.Smallint },
        { "numeric", NpgsqlDbType.Numeric },
        { "real", NpgsqlDbType.Real },
        { "double precision", NpgsqlDbType.Double },
        { "boolean", NpgsqlDbType.Boolean },
        { "text", NpgsqlDbType.Text },
        { "character varying", NpgsqlDbType.Varchar },
        { "character", NpgsqlDbType.Char },
        { "date", NpgsqlDbType.Date },
        { "timestamp", NpgsqlDbType.Timestamp },
        { "timestamp with time zone", NpgsqlDbType.TimestampTz },
        { "time", NpgsqlDbType.Time },
        { "time with time zone", NpgsqlDbType.TimeTz },
        { "bytea", NpgsqlDbType.Bytea },
        { "json", NpgsqlDbType.Json },
        { "jsonb", NpgsqlDbType.Jsonb },
        { "uuid", NpgsqlDbType.Uuid },
        { "array", NpgsqlDbType.Array },
        { "point", NpgsqlDbType.Point },
        { "line", NpgsqlDbType.Line },
        { "lseg", NpgsqlDbType.LSeg },
        { "box", NpgsqlDbType.Box },
        { "path", NpgsqlDbType.Path },
        { "polygon", NpgsqlDbType.Polygon },
        { "circle", NpgsqlDbType.Circle },
        { "cidr", NpgsqlDbType.Cidr },
        { "inet", NpgsqlDbType.Inet },
        { "macaddr", NpgsqlDbType.MacAddr },
        { "tsvector", NpgsqlDbType.TsVector },
        { "tsquery", NpgsqlDbType.TsQuery },
        { "xml", NpgsqlDbType.Xml },
        { "money", NpgsqlDbType.Money },
        { "interval", NpgsqlDbType.Interval },
        { "bit", NpgsqlDbType.Bit },
        { "varbit", NpgsqlDbType.Varbit },
        { "serial", NpgsqlDbType.Integer }, 
        { "bigserial", NpgsqlDbType.Bigint },
        { "smallserial", NpgsqlDbType.Smallint }
    };

    public static NpgsqlDbType GetNpgsqlDbType(string typeName)
    {
        typeName = typeName.ToLower();

        if (typeMatching.TryGetValue(typeName, out var npgsqlType))
        {
            return npgsqlType;
        }

        throw new ArgumentException($"Unsupported PostgreSQL data type: {typeName}");
    }

    public static List<string> GetSqlTypeNames()
    {
        return typeMatching.Keys.ToList();
    }
}
