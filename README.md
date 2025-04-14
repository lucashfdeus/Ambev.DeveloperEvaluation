# 🚀 Documentação do Projeto Developer Evaluation

## 🌟 Visão Geral
O projeto **Developer Evaluation** é uma aplicação WebAPI para gerenciamento de vendas com regras de desconto baseadas na quantidade de itens vendidos.

---

## 📋 Pré-requisitos
- 🐳 Docker instalado
- ⚙️ .NET SDK (versão compatível com o projeto)
- 🐘 PostgreSQL client (opcional para inspeção direta do banco)

---

## 🛠️ Configuração do Ambiente

### 1. Banco de Dados
🔹 Utilizaremos **apenas PostgreSQL**  
🔹 Os outros serviços (MongoDB e Redis) estão definidos no docker-compose mas não serão utilizados

### 2. Inicialização dos Containers
# 🐳 Executando o Projeto com Docker

Aqui está o guia para iniciar o projeto usando o arquivo de script:

## 🖱️ Passo a Passo para Execução

1. **Localize a Solution** no Explorador de Soluções do Visual Studio

2. **Clique com o botão direito** do mouse em cima da solution (não em um projeto específico)

3. **Selecione "Abrir Terminal"** ou "Abrir no Explorador de Arquivos"

4. No terminal ou explorador, **digite o comando**:
   ```cmd
   .\script-docker-compose-database.bat
   ```
5. **Aguarde a execução** - você verá o progresso no terminal
6. ## 📂 Arquivo `script-docker-compose-database.bat`
```batch
@echo off
echo ******************************************
echo * Iniciando containers Docker...         *
echo ******************************************

docker-compose -f docker-compose.database.yml up -d

echo ******************************************
echo * Containers iniciados com sucesso!      *
echo *                                        *
echo * WebAPI: http://localhost:8080/swagger  *
echo * PostgreSQL porta: 5432                 *
echo ******************************************

pause
```

## ✅ O que acontece quando executa o script

1. 🔄 Inicia os containers Docker em modo detached (-d)
2. 🐘 Cria o container do PostgreSQL com as configurações:
   - Database: `developer_evaluation`
   - Usuário: `developer`
   - Senha: `ev@luAt10n`
3. 🌐 Configura a WebAPI para rodar nas portas 8080 (HTTP) e 8081 (HTTPS)
4. 📊 Pronto para usar! O sistema estará disponível em:
   ```
   http://localhost:8080/swagger
   ```

## ⚠️ Possíveis Problemas e Soluções

1. **Erro "Arquivo não encontrado"**:
   - Verifique se o script está na raiz da solution
   - Confira se você está executando do diretório correto

2. **Erros de permissão**:
   - Execute o terminal/PowerShell como administrador
   - Verifique se o Docker Desktop está rodando

3. **Portas em uso**:
   ```cmd
   netstat -ano | findstr :8080
   netstat -ano | findstr :5432
   ```
   - Se estiverem em uso, altere no docker-compose.yml

## 🔍 Verificando os Containers
Após executar, verifique se tudo está OK com:
```cmd
docker ps
```

Você deverá ver o container:
- `ambev_developer_evaluation_database`

Pronto! Seu ambiente está configurado e pronto para desenvolvimento 🎉

🔄 Isso irá iniciar:
- 🌐 WebAPI da aplicação (na porta 8080/8081)
- 💾 Banco de dados PostgreSQL (na porta 5432)

---

## 🏗️ Gerenciamento de Migrations

### 📌 Criar uma nova Migration
```bash
Add-Migration [NOME_DA_MIGRATION] -Context DefaultContext -Project Ambev.DeveloperEvaluation.ORM -StartupProject Ambev.DeveloperEvaluation.WebApi -OutputDir "Migrations"
```

💡 Exemplo:
```bash
Add-Migration _13042025SaleInicial -Context DefaultContext -Project Ambev.DeveloperEvaluation.ORM -StartupProject Ambev.DeveloperEvaluation.WebApi -OutputDir "Migrations"
```

### ⬆️ Aplicar Migrations ao Banco de Dados
```bash
Update-Database -Migration [NOME_DA_MIGRATION] -Context DefaultContext
```

💡 Exemplo:
```bash
Update-Database -Migration _13042025SaleInicial -Context DefaultContext
```

---

## 📨 Exemplos de Requests

### 1. 🛒 Venda Válida Básica (Sem Desconto)
```json
{
  "saleNumber": "SALE-001",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "John Doe",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Downtown Branch",
  "saleDate": "2025-04-12T10:00:00Z",
  "items": [
    {
      "productId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "productName": "Product A",
      "quantity": 3,
      "unitPrice": 10.50
    }
  ]
}
```

### 2. 💰 Venda com Desconto de 10% (4-9 itens)
```json
{
  "saleNumber": "SALE-002",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Jane Smith",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Uptown Branch",
  "saleDate": "2025-04-12T11:30:00Z",
  "items": [
    {
      "productId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "productName": "Product B",
      "quantity": 5,
      "unitPrice": 20.00
    }
  ]
}
```

### 3. 🎁 Venda com Desconto de 20% (10-20 itens)
```json
{
  "saleNumber": "SALE-003",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Acme Corp",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Industrial Branch",
  "saleDate": "2025-04-12T14:15:00Z",
  "items": [
    {
      "productId": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
      "productName": "Product C",
      "quantity": 12,
      "unitPrice": 15.75
    }
  ]
}
```

### 4. 🛍️ Venda com Múltiplos Itens e Descontos Diferentes
```json
{
  "saleNumber": "SALE-004",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Global Distributors",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Main Branch",
  "saleDate": "2025-04-12T16:45:00Z",
  "items": [
    {
      "productId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "productName": "Product A",
      "quantity": 2,
      "unitPrice": 10.00
    },
    {
      "productId": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
      "productName": "Product B",
      "quantity": 7,
      "unitPrice": 25.00
    },
    {
      "productId": "7fa85f64-5717-4562-b3fc-2c963f66afb0",
      "productName": "Product C",
      "quantity": 15,
      "unitPrice": 30.00
    }
  ]
}
```

### 5. ❌ Cenário de Erro - Limite de Quantidade Excedido
```json
{
  "saleNumber": "SALE-005",
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "Test Customer",
  "branchId": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "branchName": "Test Branch",
  "saleDate": "2025-04-12T18:00:00Z",
  "items": [
    {
      "productId": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
      "productName": "Product X",
      "quantity": 21,
      "unitPrice": 10.00
    }
  ]
}
```

---

## 🔍 Acesso ao Banco de Dados
- **Host**: `localhost`
- **Porta**: `5432`
- **Database**: `developer_evaluation`
- **Usuário**: `developer`
- **Senha**: `ev@luAt10n`

---

## 🔌 Endpoints da API
- `📤 POST /api/sales` - Criar uma nova venda
- `📥 GET /api/sales` - Listar todas as vendas
- `🔍 GET /api/sales/{id}` - Obter detalhes de uma venda específica

---

## 📊 Regras de Negócio
- **Descontos** são aplicados com base na quantidade de itens:
  - 🟢 1-3 itens: 0% de desconto
  - 🔵 4-9 itens: 10% de desconto
  - 🟡 10-20 itens: 20% de desconto
  - 🔴 Mais de 20 itens: não permitido (erro)

---

## 🚨 Solução de Problemas
Se encontrar problemas ao iniciar os containers:
1. 🔄 Verifique se o Docker está rodando
2. 🔌 Confira se as portas 8080, 8081 e 5432 estão disponíveis
3. 📜 Execute `docker-compose logs` para ver os logs dos serviços

---

## 🤝 Contribuição
✨ Sinta-se à vontade para contribuir com melhorias no projeto!
