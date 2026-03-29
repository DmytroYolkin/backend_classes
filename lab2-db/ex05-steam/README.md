# Game Model Properties in PostgreSQL

In this project, the `Game` model uses three specific properties that store data differently than standard relational columns. These properties take advantage of PostgreSQL's advanced, native features to optimize storage and querying:

## 1. `Tags` (Stored as `jsonb`)
```csharp
[Column(TypeName = "jsonb")]
public List<string> Tags { get; set; } = new();
```
- **What it is:** This property uses PostgreSQL's `JSONB` (binary JSON) data type.
- **Why it's used:** Instead of creating a separate table for tags and strictly enforcing a many-to-many relationship, `JSONB` allows the application to store an array of strings directly in a single column. It parses the JSON representation into a binary format, making retrieval fast, and allows you to query directly inside the array using specialized JSON operators.

## 2. `Requirements` (Flattened / JSONB Complex Type)
```csharp
public SystemRequirements Requirements { get; set; } = new();
```
- **What it is:** A complex nested object (`SystemRequirements`) that is either flattened into individual columns (Owned Entity Type) or stored as a single `JSONB` column.
- **Why it's used:** It prevents the need for a separate related table (like a `Requirements` table with a foreign key). By mapping complex types directly onto the parent table's columns or storing them as a JSON object, the data can be retrieved in a single query without complex `JOIN` statements, maintaining an object-oriented model in C# while optimizing database interactions.

## 3. `SearchVector` (Stored as `tsvector` / `NpgsqlTsVector`)
```csharp
public NpgsqlTsVector SearchVector { get; set; } = null!;
```
- **What it is:** A property mapped to PostgreSQL's `tsvector` data type for Full-Text Search.
- **Why it's used:** Standard SQL `LIKE '%word%'` queries are extremely slow on large bodies of text because they require full table scans and cannot use traditional indexes efficiently. A `tsvector` parses and stores text as sorted, normalized "lexemes" (words). When combined with a GIN index, this allows for extremely fast, natural-language full-text searches (searching for plural/singular forms, ignoring stop words, ranking results relevance) directly on the Steam dataset.