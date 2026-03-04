# Setup Docker Compose

## Como executar

1. Certifique-se de ter Docker e Docker Compose instalados

2. Execute o comando para subir todos os serviços:
```bash
docker-compose up --build
```

3. Acesse o frontend em: `http://localhost:3000`

4. A API estará disponível em: `http://localhost:8080`

## Serviços

- **Frontend**: Porta 3000 (React/Vite)
- **Backend API**: Porta 8080 (.NET 8)
- **PostgreSQL**: Porta 5432

## Estrutura

O frontend está configurado para fazer proxy reverso através do nginx para a API quando executado no Docker. Quando executado localmente, ele usa `http://localhost:8080/api/v1` diretamente.

## Variáveis de Ambiente

O frontend detecta automaticamente se está rodando no Docker ou localmente e ajusta a URL da API conforme necessário.
