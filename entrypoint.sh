export PATH="$PATH:/root/.dotnet/tools"
dotnet tool install --global dotnet-ef --version 3.1.12
dotnet-ef --startup-project Blog/Blog.csproj migrations add InitialModel -p DAL/DAL.csproj
dotnet-ef --startup-project Blog/Blog.csproj database update InitialModel -p DAL/DAL.csproj
dotnet run --project Blog/Blog.csproj