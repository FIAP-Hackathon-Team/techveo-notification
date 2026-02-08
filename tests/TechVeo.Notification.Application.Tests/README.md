# TechVeo.Notification.Application.Tests

Este projeto contém os testes de unidade para o projeto TechVeo.Notification seguindo o padrão estabelecido no projeto TechVeo.Management.

## Estrutura do Projeto

```
TechVeo.Notification.Application.Tests/
├── Events/
│   └── Integration/
│       └── Incoming/
│           └── Handlers/
│               └── ProcessEmailEventHandlerTests.cs
├── Fixtures/
│   └── EmailEventFixture.cs
├── Services/
│   └── SendEmailServiceTests.cs
├── GlobalUsings.cs
└── TechVeo.Notification.Application.Tests.csproj
```

## Padrões Utilizados

### Bibliotecas de Teste
- **xUnit**: Framework de testes
- **Moq**: Biblioteca para criação de mocks
- **FluentAssertions**: Asserções fluentes para melhor legibilidade
- **Bogus**: Geração de dados fake (disponível para uso futuro)

### Convenções de Nomenclatura

#### Classes de Teste
- Sufixo `Tests` no nome da classe
- Exemplo: `ProcessEmailEventHandlerTests`, `SendEmailServiceTests`

#### Métodos de Teste
- Nomenclatura descritiva em inglês
- Formato: `<Should/ShouldNot>_<Action>_<Condition>`
- Exemplos:
  - `Handle_WithCompletedStatus_ShouldSendSuccessEmail`
  - `Handle_WithFailedStatus_ShouldSendFailureEmail`

#### Atributos
- `[Fact]`: Para testes simples
- `[DisplayName("...")]`: Descrição legível do teste
- `[Trait("Category", "SubCategory")]`: Categorização dos testes
  - Exemplos: `[Trait("Application", "ProcessEmailEventHandler")]`

### Estrutura de Teste (AAA Pattern)

Todos os testes seguem o padrão **Arrange-Act-Assert**:

```csharp
[Fact(DisplayName = "Should send success email when status is Completed")]
[Trait("Application", "ProcessEmailEventHandler")]
public async Task Handle_WithCompletedStatus_ShouldSendSuccessEmail()
{
    // Arrange - Configuração dos objetos e dependências
    var emailAddress = "user@example.com";
    var emailEvent = new EmailEvent(emailAddress, fileName, StatusType.Completed, url);

    // Act - Execução do método sendo testado
    await _handler.Handle(emailEvent, CancellationToken.None);

    // Assert - Verificação dos resultados
    _sendEmailServiceMock.Verify(
        x => x.SendAsync(emailAddress, It.IsAny<string>(), It.IsAny<string>()),
        Times.Once);
}
```

### Fixtures

As fixtures são usadas para criar objetos de teste de forma reutilizável:

```csharp
public class EmailEventFixture
{
    public EmailEvent CreateCompletedEmailEvent() { ... }
    public EmailEvent CreateFailedEmailEvent() { ... }
}
```

#### Uso de Fixtures
```csharp
public class MyTests : IClassFixture<EmailEventFixture>
{
    private readonly EmailEventFixture _fixture;

    public MyTests(EmailEventFixture fixture)
    {
        _fixture = fixture;
    }
}
```

## Testes Implementados

### ProcessEmailEventHandlerTests
Testa o handler responsável por processar eventos de email:
- ✅ Envio de email de sucesso quando status é Completed
- ✅ Envio de email de falha quando status é Failed
- ✅ Verificação do assunto do email
- ✅ Chamada única do método SendAsync
- ✅ Tratamento de valores vazios (fileName, URL)

### SendEmailServiceTests
Testa o serviço de envio de emails:
- ✅ Criação do serviço com configuração válida
- ✅ Uso do endereço de origem configurado
- ✅ Uso das opções AWS
- ✅ Tratamento de parâmetros vazios

## GlobalUsings.cs

Imports globais para todos os arquivos de teste:
```csharp
global using Xunit;
global using Moq;
global using FluentAssertions;
```

## Executando os Testes

### Todos os testes
```bash
dotnet test
```

### Com detalhes verbosos
```bash
dotnet test --verbosity detailed
```

### Por categoria
```bash
dotnet test --filter "Category=Application"
```

### Testes específicos
```bash
dotnet test --filter "FullyQualifiedName~ProcessEmailEventHandler"
```

## Cobertura de Código

Para gerar relatório de cobertura:
```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Notas Importantes

1. **InternalsVisibleTo**: O projeto Application possui o atributo `InternalsVisibleTo` configurado para permitir que os testes acessem classes internas.

2. **Mocking do Logger**: O logger é mockado usando `Mock<ILogger<T>>` para evitar dependências desnecessárias.

3. **Async/Await**: Todos os testes assíncronos retornam `Task` e utilizam `async/await` corretamente.

4. **Verificações com Moq**: Use `Verify()` para validar chamadas de métodos mockados:
   ```csharp
   _mock.Verify(x => x.Method(It.IsAny<Type>()), Times.Once);
   ```

5. **FluentAssertions**: Use para asserções mais legíveis:
   ```csharp
   result.Should().NotBeNull();
   result.Should().Be(expected);
   ```

## Próximos Passos

- [ ] Adicionar testes de integração
- [ ] Implementar testes com dados parametrizados usando `[Theory]`
- [ ] Adicionar testes para cenários de erro/exceção
- [ ] Implementar relatórios de cobertura de código
- [ ] Adicionar testes de performance se necessário
