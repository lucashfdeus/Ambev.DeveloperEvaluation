# 🚀 Documentação do Projeto Developer Evaluation

## 🌟 Visão Geral
O projeto **Developer Evaluation** é uma aplicação WebAPI para gerenciamento de vendas com regras de desconto baseadas na quantidade de itens vendidos.

---

## 📋 Pré-requisitos
- 🐳 Docker instalado e rodando
- ⚙️ .NET SDK 6.0 ou superior
- 🐘 PostgreSQL client (opcional para debug avançado)
- 🧰 Visual Studio 2022 ou VS Code (recomendado)

---

## 🛠️ Configuração do Ambiente

### 1. Banco de Dados
🔹 **PostgreSQL** como banco principal  
🔹 Configuração automática via Docker Compose

### 2. Inicialização dos Containers
## 🐳 Executando o Projeto com Docker

### 🖱️ Passo a Passo Simplificado
1. **Abra o terminal** na raiz do projeto
2. **Execute o script**:
   ```cmd
   .\script-docker-compose-database.bat
   ```
3. **Aguarde** a mensagem de confirmação

### 📜 Conteúdo do Script
```batch
@echo off
echo ******************************************
echo * 🐋 Iniciando containers Docker...      *
echo ******************************************

docker-compose -f docker-compose.database.yml up -d

echo ******************************************
echo * ✅ Containers iniciados com sucesso!    *
echo *                                        *
echo * 🌐 WebAPI: http://localhost:8080/swagger
echo * 🐘 PostgreSQL porta: 5432             *
echo ******************************************
pause
```

### 🔍 Verificação Pós-Instalação
```cmd
docker ps
```
Deve mostrar:
- `ambev_developer_evaluation_database`
  
## 🏗️ Gerenciamento de Migrations
### Package Manager Console
```powershell
Update-Database -Context DefaultContext
```
### Terminal .NET CLI
```bash
dotnet ef database update --context DefaultContext
```
---
## 🐞 Iniciando em Modo Debug
1. **Abra a solution** no Visual Studio
2. **Selecione o perfil** `Ambev.DeveloperEvaluation.WebApi - HTTPS`
3. **Pressione F5** ou clique no botão ▶️
4. **Acesse** o Swagger em:
   ```
   https://localhost:[PORT]/swagger
   ```
> 💡 Dica: Configure breakpoints antes de iniciar para debug eficiente
---
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
- `🔍 GET /api/sales/{id}` - Obter detalhes de uma venda específica
- `📥 DELETE /api/sales/{id}` - Deletar uma venda específica
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
