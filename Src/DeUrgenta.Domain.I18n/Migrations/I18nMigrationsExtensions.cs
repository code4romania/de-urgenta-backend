using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace DeUrgenta.Domain.I18n.Migrations
{
    internal static class I18nMigrationsExtensions
    {
        internal static OperationBuilder<SqlOperation> AddTranslation(
            this MigrationBuilder migrationBuilder,
            Guid languageId,
            string key,
            string value)
            => migrationBuilder.Sql($@"INSERT INTO i18n.""StringResources""(""Key"", ""Value"", ""LanguageId"") VALUES ('{key}', '{value}', '{languageId}');");

        internal static OperationBuilder<SqlOperation> RemoveTranslation(
            this MigrationBuilder migrationBuilder,
            Guid languageId,
            string key)
            => migrationBuilder.Sql($@"DELETE FROM i18n.""StringResources"" WHERE ""LanguageId"" = '{languageId}' AND  ""Key"" = '{key}';");

    }
}