## Entity Framework install

<pre>
dotnet tool install --global dotnet-ef --version 8.*
</pre>

## Add Migration

<pre>
dotnet ef migrations add {name} --project React_dotnet.database --startup-project React_dotnet.Server --context CoreDbContext
</pre>
