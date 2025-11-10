using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using skbd.Models;







namespace skbd
{




    public class ValidationTests
    {
        [Theory]
        [InlineData("123", 1, true)]   // int
        [InlineData("abc", 1, false)]  // int
        [InlineData("12.5", 2, true)]  // double
        [InlineData("x", 3, true)]     // char
        [InlineData("xyz", 3, false)]  // char
        public void TestValidateValue(string val, int typeId, bool expected)
        {
            var form = new skbd.Form1();
            var result = form.ValidateValue(val, typeId);
            Assert.Equal(expected, result);
        }
    }


    public class RowService
    {
        private readonly SubdContext _context;

        public RowService(SubdContext context)
        {
            _context = context;
        }

        public Row AddNewRow(int tableId)
        {
            // 1. Створюємо рядок
            var newRow = new Row
            {
                TableId = tableId
            };
            _context.Rows.Add(newRow);
            _context.SaveChanges(); // тут newRow.Id генерується

            // 2. Для кожного поля створюємо Value
            foreach (var field in _context.Fields.Where(f => f.TableId == tableId))
            {
                var newValue = new Value
                {
                    RowId = newRow.Id,
                    FieldId = field.Id,
                    Val = "" // пустий рядок
                };
                _context.Values.Add(newValue);
            }

            _context.SaveChanges();

            return newRow;
        }
    }




    public class RowServiceTests
    {
        private SubdContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SubdContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new SubdContext(options);
        }

        [Fact]
        public void AddNewRow_ShouldCreateRowAndValues()
        {
            using var context = GetInMemoryContext();

            // Arrange: додаємо таблицю і поля
            var table = new Table { Name = "TestTable" };
            context.Tables.Add(table);
            context.SaveChanges();

            context.Fields.Add(new Field { TableId = table.Id, Name = "Field1" });
            context.Fields.Add(new Field { TableId = table.Id, Name = "Field2" });
            context.SaveChanges();

            var service = new RowService(context);

            // Act
            var row = service.AddNewRow(table.Id);

            // Assert
            Assert.NotNull(row);
            Assert.True(row.Id > 0);

            var values = context.Values.Where(v => v.RowId == row.Id).ToList();
            Assert.Equal(2, values.Count);              // для кожного поля є Value
            Assert.All(values, v => Assert.Equal("", v.Val)); // значення пусті строки
        }
    }



    public class TableDiffService
    {
        private readonly SubdContext _context;

        public TableDiffService(SubdContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Повертає рядки з таблиці tableId1, які не мають відповідників у tableId2 по заданих полях.
        /// </summary>
        public List<Row> GetDifferentRows(int tableId1, string field1Name, int tableId2, string field2Name)
        {
            var field1 = _context.Fields.FirstOrDefault(f => f.TableId == tableId1 && f.Name == field1Name);
            var field2 = _context.Fields.FirstOrDefault(f => f.TableId == tableId2 && f.Name == field2Name);

            if (field1 == null || field2 == null)
                throw new InvalidOperationException("Field not found");

            if (field1.TypeId != field2.TypeId)
                throw new InvalidOperationException("Field types do not match");

            var values1 = _context.Values.Where(v => v.Field.TableId == tableId1).ToList();
            var values2 = _context.Values.Where(v => v.Field.TableId == tableId2).ToList();

            var rowIds1 = values1.Select(v => v.RowId).Distinct().ToList();
            var rowIds2 = values2.Select(v => v.RowId).Distinct().ToList();

            List<Row> result = new List<Row>();

            foreach (var rowId1 in rowIds1)
            {
                string val1 = values1.FirstOrDefault(v => v.RowId == rowId1 && v.FieldId == field1.Id)?.Val?.Trim();

                bool hasMatch = rowIds2.Any(rowId2 =>
                    val1 == values2.FirstOrDefault(v => v.RowId == rowId2 && v.FieldId == field2.Id)?.Val?.Trim());

                if (!hasMatch)
                {
                    var row = _context.Rows.First(r => r.Id == rowId1);
                    result.Add(row);
                }
            }

            return result;
        }
    }


    public class TableDiffServiceTests
    {
        private SubdContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SubdContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // кожен тест своя БД
                .Options;

            var context = new SubdContext(options);

            // Створюємо тестові дані
            var table1 = new Table { Id = 1, DbId = 1, Name = "Table1" };
            var table2 = new Table { Id = 2, DbId = 1, Name = "Table2" };
            var type = new Models.Type { Id = 1, Name = "string" };

            context.Tables.AddRange(table1, table2);
            context.Types.Add(type);

            var f1 = new Field { Id = 1, TableId = 1, Name = "Col1", TypeId = 1 };
            var f2 = new Field { Id = 2, TableId = 2, Name = "ColX", TypeId = 1 };

            context.Fields.AddRange(f1, f2);

            var r1 = new Row { Id = 1, TableId = 1 };
            var r2 = new Row { Id = 2, TableId = 1 };
            var r3 = new Row { Id = 3, TableId = 2 };

            context.Rows.AddRange(r1, r2, r3);

            context.Values.AddRange(
                new Value { RowId = 1, FieldId = 1, Val = "A" },
                new Value { RowId = 2, FieldId = 1, Val = "B" },
                new Value { RowId = 3, FieldId = 2, Val = "A" }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void GetDifferentRows_ShouldReturnRowsWithoutMatches()
        {
            using var context = GetInMemoryContext();
            var service = new TableDiffService(context);

            var diff = service.GetDifferentRows(1, "Col1", 2, "ColX");

            Assert.Single(diff);              // тільки один рядок відрізняється
            Assert.Equal(2, diff[0].Id);      // це Row з Val = "B"
        }
    }



}











