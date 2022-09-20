# Gerador de Tickets CSharp
Realizado no modo de console e banco de dados SQL Server

------------
***Usabilidade***

Tratado para menu ser acionado com digitos númericos.

### Funções do Menu<br/>

###### 1.Manutenção de Funcionários<br/>
  ###### 1.1 Adicionar Novo <br/>
Adiciona um novo funcionário não permitindo seu cadastro quando o CPF digitado já está cadastrado no banco de dados.

  ###### 1.2 Editar<br />
   ###### 1.2.1 Edição por nome<br/>
Procura no banco de dados o nome digitado e edita para o novo nome informado.<br/>
  ###### 1.2.2 Edição por CPF<br/>
Procura no banco de dados o CPF digitado e altera para o novo CPF informado. Valida se o CPF digitado já existe.<br/>
   ###### 1.2.3 Edição de Situação<br/>
Edita um cadastro para Ativo ou Inativo. 

###### 2.Manuteção de Tickets<br/>
###### 2.1 Adicionar Novo<br/>
Novos Tickets são adicionados por CPF por serem únicos por funcionários.<br/>
###### 2.2 Editar<br/>
###### 2.2.1 Edição de quantidade entregue<br/>
Atualizar em um ticket a quantidade entregue ao funcionário.<br/>
###### 2.2.2 Edição de funcionário que foi entregue <br/>
Atualiza pelo número do Ticket o funcionário que recebeu um Ticket.
###### 2.2.3 Edição de situação
Ativa ou Inativa um ticket entregue.<br/>

###### 3.Relatórios<br/>
###### 3.1 Total por funcionário<br/>
Exibe todos os tickets entregue para um determinado funcionário.<br/>
###### 3.2 Total por funcionário e período<br/>
Exibe todos os tickets entregue para um funcionário filtrando pela data escolhida.<br/>
###### 3.3 Total por período<br/>
Exibe todos os tickets entregues na data determinada.<br/>








------------
***Comunicação com banco de dados***

A classe Conexão retém as configurações relacionadas ao banco de dados.

A Connection String na linha 17 deve apontar seu servidor.

//("Data Source=PC\\SERVIDOR;Initial Catalog=BANCO DE DADOS;User ID=USUARIO;Password=SENHA"));


Tabela Funcionarios

- Coluna ID_FUNC sendo sua PK.
- Nome e Cpf NotNull.

Tabela Ticket

- Coluna ID_TICKET sendo sua PK.
- Coluna FUNCIONARIO é uma FK referenciando a ID_FUNC.
- Coluna FUNCIONARIO e QUANTIDADE NOT_NULL.



------------
***Futuras melhorias***

- Tratamento em campo de CPF para permitir 11 caracteres e validações referentes à seus digitos para verificar sua integridade.
- Campos de data serão tratados para permitidas datas válidas.
- Clean Code geral, principalmente na parte de ManutencaoFuncionarios(), deve-se usar uma classe centralizando todos seus operandos.




