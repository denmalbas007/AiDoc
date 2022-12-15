using System;
using System.Data;
using Dapper;

namespace AiDoc.Api.DataAccess;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid guid)
    {
        parameter.Value = guid.ToString();
    }

    public override Guid Parse(object value)
        => new Guid((string)value);
}