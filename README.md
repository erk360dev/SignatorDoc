
# SignatorDoc

**SignatorDoc** é um sistema avançado de assinatura digital de documentos PDF que combina **certificados digitais** com **assinatura manuscrita biométrica**.

---

## ✨ Destaques

- 🔐 Assinatura digital com certificados (ICP-Brasil)
- ✍️ Captura de assinatura manuscrita com dados biométricos
- 🧠 Ferramenta de análise grafotécnica da assinatura
- 📍 Posicionamento automático ou manual de campos de assinatura
- 🔁 Replicação e duplicação de assinaturas em várias páginas
- 📂 Assinatura em lote de documentos
- 🔑 Proteção por senha em arquivos PDF
- 🖼️ Inserção de selos/carimbos como imagem de assinatura

---

## 🛠️ Funcionalidades

### Assinatura Digital + Biométrica
- Combina criptografia e certificação digital com dados como pressão, velocidade e tempo da escrita.
- Garante **imutabilidade** do documento após assinatura.

### Captura com Mesa de Assinatura
- Compatível com dispositivos como **Wacom**, que capturam:
  - Pressão da caneta
  - Velocidade do traço
  - Tempo e inclinação

### Validação de Integridade
- Caso o documento seja alterado após assinado, a assinatura é invalidada automaticamente.

### Posicionamento Inteligente
- Insere campos de assinatura:
  - Manualmente (posição livre)
  - Por mapeamento (ex: "superior esquerdo")
  - Por palavras-chave (ex: “Testemunha”, “____”)

### Assinatura em Lote
- Permite assinar múltiplos documentos de uma vez.
- Visualização em grade para controle de páginas e assinaturas.

### Análise Grafotécnica
- Visualização gráfica dos traços da assinatura.
- Permite perícia técnica em casos de contestação.

---

## 🧱 Estrutura do Projeto

O repositório é organizado em três subprojetos principais, cada um com uma função específica dentro da arquitetura do **SignatorDoc**:

### 📁 `GhostScriptUtils`  
Módulo responsável pela manipulação de documentos PDF em ambientes onde o **Adobe Acrobat** não está instalado. Utiliza o mecanismo do **Ghostscript** para operações como renderização, conversão e tratamento de arquivos PDF de forma autônoma.

### 📁 `NativeDeviceControlLib`  
Biblioteca dedicada ao controle de dispositivos de captura de assinatura, como mesas digitalizadoras (ex: **Wacom**). Realiza a interface de comunicação com o hardware, capturando dados biométricos como pressão, velocidade e tempo dos traços.

### 📁 `SignatorDocSolution`  
Projeto principal da aplicação. Contém toda a interface gráfica, controle das telas, lógica de negócios e integração com as ferramentas de assinatura digital, assinatura manuscrita e validação dos documentos.

---

## 📦 Instalação

Clone este repositório, abra no Visual Studio, compile e execute.

```bash
git clone https://github.com/erk360dev/SignatorDoc.git
```

---

## ✅ Requisitos

- 💻 **Windows**
- 🧰 **Visual Studio 2019** ou superior
- 📚 **.NET Framework 4.0**
- ✍️ **Drivers da mesa de assinatura** (Wacom ou equivalente)
- 🔐 **Certificado digital tipo A1 ou A3** (para testes com assinatura digital)

---

## ▶️ Passos para executar

1. Faça o clone do repositório:

   ```bash
   git clone https://github.com/erk360dev/SignatorDoc.git
   ```

2. Abra o arquivo `SignatorDoc.sln` no Visual Studio  
3. Compile o projeto com **Ctrl + Shift + B**  
4. Execute com **F5**  
5. Utilize a interface do sistema para importar PDFs, configurar assinaturas e aplicar selos ou certificados

---

## 🧪 Exemplo de Uso

1. Abra o sistema e carregue um documento PDF  
2. Posicione os campos de assinatura manualmente ou via busca por palavras-chave  
3. Capture a assinatura manuscrita em tempo real usando a mesa de assinatura  
4. Selecione e aplique seu certificado digital (A1 ou A3)  
5. Salve o documento assinado com proteção e integridade garantidas  
6. Use a ferramenta de análise para visualizar curvas de pressão e tempo  

---

## 📜 Licença

Este projeto está disponível para fins educacionais e institucionais.  
Consulte o arquivo `LICENSE` para mais informações.

---

## 🌐 Website Oficial

**[http://em-manutencao.temp](http://em-manutencao.temp/signatordoc)**  
> *Sistema SignatorDoc.*

---

## 🤝 Contribuições

Contribuições são bem-vindas.  
Abra um **issue** ou envie um **pull request** com sugestões, correções ou melhorias.
Tenho planos de modernizar para javascript com algum framework front e c# ou c++ em back, em caso de interesse entrar em contato pelo email erk360dev@hotmail.com
