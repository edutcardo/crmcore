using CRMCore.Module.GraphQL.Models;
using CRMCore.Module.GraphQL.Resolvers;
using CRMCore.Module.GraphQL.Types;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace CRMCore.Module.GraphQL
{
    public class GraphQLQuery : ObjectGraphType<object>
    {
        private IDatabaseMetadata _dbMetadata;
        private ITableNameLookup _tableNameLookup;
        private DbContext _dbContext;

        public GraphQLQuery(
            DbContext dbContext, 
            IDatabaseMetadata dbMetadata,
            ITableNameLookup tableNameLookup)
        {
            _dbMetadata = dbMetadata;
            _tableNameLookup = tableNameLookup;
            _dbContext = dbContext;

            Name = "Query";

            foreach (var metaTable in _dbMetadata.GetTableMetadatas())
            {
                var tableType = new TableType(metaTable);
                var friendlyTableName = _tableNameLookup.GetFriendlyName(metaTable.TableName);

                AddField(new FieldType
                {
                    Name = friendlyTableName,
                    Type = tableType.GetType(),
                    ResolvedType = tableType,
                    Resolver = new MyFieldResolver(metaTable, _dbContext),
                    Arguments = new QueryArguments(
                        tableType.TableArgs
                    )
                });

                // lets add key to get list of current table
                var listType = new ListGraphType(tableType);
                AddField(new FieldType
                {
                    Name = $"{friendlyTableName}_list",
                    Type = listType.GetType(),
                    ResolvedType = listType,
                    Resolver = new MyFieldResolver(metaTable, _dbContext),
                    Arguments = new QueryArguments(
                        tableType.TableArgs                        
                    )
                });
            }
        }
    }
}
