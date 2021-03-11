# OrleansDemo

```powershell
ng new UI
cd UI
npm install
ng add @microsoft/signalr
ng add @angular/material
ng build

ng g service signal-r
```

csproj:
```xml
    <ItemGroup>
        <PackageReference Include="Microsoft.AspNetCore.SpaServices.Extensions" Version="5.0.3" />
        <Content Remove="UI\**" />
        <None Remove="UI\**" />
        <None Include="UI\**" Exclude="UI\node_modules\**" />
    </ItemGroup>
```

```c#
services.AddSpaStaticFiles(options => options.RootPath = "UI/dist/UI");

app.UseSpaStaticFiles();

app.UseSpa(_ => { });
```
