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
- Login (`admin` / `Admin@123`)
- CRUD de Igrejas
- CRUD de Organistas por igreja
- Cadastro de dias por tipo de culto
- Geração e impressão de rodízio por período e tipo (Reunião de Jovens, Cultos Oficiais)
- Validação client-side e server-side
- Paginação/filtro via DataTables e repositórios paginados

## Rodar com Docker
```bash
docker compose up --build
```
Acesse: `http://localhost:5000`

## Rodar no Ubuntu (sem Docker)
1. Instalar .NET 10 SDK/runtime (preview) e MySQL 8.
2. Restaurar e publicar:
```bash
dotnet restore
cd src/RodizioOrganistas.Web
dotnet publish -c Release -o /opt/rodizio-organistas
```
3. Copiar `deploy/systemd/rodizio-organistas.service` para `/etc/systemd/system/` e ajustar variáveis.
4. Habilitar serviço:
```bash
sudo systemctl daemon-reload
sudo systemctl enable rodizio-organistas
sudo systemctl start rodizio-organistas
```
5. Configurar Nginx com `deploy/nginx/rodizio-organistas.conf`.
```bash
sudo ln -s /etc/nginx/sites-available/rodizio-organistas.conf /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```
