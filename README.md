# DestinyMatch - Back End Repository

- Scafford:

- Package Manage Console:
```plaintext
Scaffold-DbContext "Server=localhost;Database=[DestinyMatch];uid=sa;pwd=12345;TrustedServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

- Terminal:
```plaintext
dotnet ef dbcontext scaffold "Server=localhost;Database=DestinyMatch;uid=sa;pwd=12345;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --force
```
