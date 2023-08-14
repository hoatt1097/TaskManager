Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation Version 5.0.9
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 5.0.9
Install-Package Microsoft.EntityFrameworkCore.SqlServer  -Version 5.0.9
Install-Package Microsoft.EntityFrameworkCore -Version 5.0.9
PagedList.Core

// Show popup
https://craftpip.github.io/jquery-confirm/

//Date picker
https://flatpickr.js.org/examples/

// Inital database: Run 2 command
 add-migration inital
 update-database

 hoatt1097 --> hash: 84d1782df8dd56d75df3b515469c9711



// Sinh model tu database - Các anotation ở trong config
Scaffold-DBContext "Server=THIENHOA;Database=db_uts;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
Scaffold-DBContext "Server=THIENHOA;Database=db_uts;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force

// Sinh model tu database - Các anotation ở trong class (Chọn cách này cho trực quan)
Scaffold-DBContext "Server=THIENHOA;Database=db_uts;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -OutputDir Models -Force
Scaffold-DBContext "Server=THIENHOA;Database=CK_POS_TEST;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -DataAnnotations -OutputDir Models -Force



// TaskManager Project Note
Add-Migration InitialMigration -c TaskManagerContext -o Data/Migrations
Update-Database
