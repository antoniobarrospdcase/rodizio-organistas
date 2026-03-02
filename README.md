# Rodízio de Organistas (ASP.NET Core MVC + EF Core + MySQL)

## Stack
- .NET 10.0 (preview)
- ASP.NET Core MVC
- Entity Framework Core
- MySQL 8
- Arquitetura em camadas + Repository Pattern
- AdminLTE + DataTables
- Autenticação via Cookie

## Funcionalidades
- Login de administrador via cookie (credenciais por configuração segura)
- CRUD de Igrejas
- CRUD de Organistas por igreja
- Cadastro de dias por tipo de culto
- Geração e impressão de rodízio por período e tipo (Reunião de Jovens, Cultos Oficiais)
- Validação client-side e server-side
- Paginação/filtro via DataTables e repositórios paginados

## Configuração de autenticação
As credenciais do administrador **não ficam no código-fonte**. Configure em segredo de ambiente:

- `Authentication__Admin__Username`
- `Authentication__Admin__PasswordHash`

O valor de `PasswordHash` deve ser gerado com `PasswordHasher` do ASP.NET Core e armazenado já em formato hash.

## Banco de dados (script inicial + EF)
- Script SQL inicial: `db/init/001_initial.sql`
- No Docker, esse script é aplicado automaticamente pelo MySQL na primeira inicialização do volume.
- A aplicação usa:
  - `Database.Migrate()` quando existirem migrations EF versionadas;
  - `Database.EnsureCreated()` como fallback quando ainda não houver migrations.

## Rodar com Docker
Defina as variáveis de ambiente `AUTH_ADMIN_USERNAME` e `AUTH_ADMIN_PASSWORD_HASH` (por exemplo em um arquivo `.env`) antes de subir a aplicação.

```bash
docker compose up --build
```
Acesse: `http://localhost:5000`

> Se quiser reinicializar do zero o banco e reaplicar o script inicial:
```bash
docker compose down -v
docker compose up --build
```

## Criar migrations EF (opcional/recomendado para produção)
```bash
dotnet tool update --global dotnet-ef
dotnet restore

dotnet ef migrations add InitialCreate \
  --project src/RodizioOrganistas.Infrastructure \
  --startup-project src/RodizioOrganistas.Web \
  --context RodizioOrganistas.Infrastructure.Data.AppDbContext \
  --output-dir Data/Migrations

dotnet ef database update \
  --project src/RodizioOrganistas.Infrastructure \
  --startup-project src/RodizioOrganistas.Web \
  --context RodizioOrganistas.Infrastructure.Data.AppDbContext
```

## Rodar no Ubuntu (sem Docker)
1. Instalar .NET 10 SDK/runtime (preview) e MySQL 8.
2. Criar o schema executando o script `db/init/001_initial.sql` no MySQL.
3. Restaurar e publicar:
```bash
dotnet restore
cd src/RodizioOrganistas.Web
dotnet publish -c Release -o /opt/rodizio-organistas
```
4. Copiar `deploy/systemd/rodizio-organistas.service` para `/etc/systemd/system/` e ajustar variáveis.
5. Habilitar serviço:
```bash
sudo systemctl daemon-reload
sudo systemctl enable rodizio-organistas
sudo systemctl start rodizio-organistas
```
6. Configurar Nginx com `deploy/nginx/rodizio-organistas.conf`.
```bash
sudo ln -s /etc/nginx/sites-available/rodizio-organistas.conf /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```
