
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

## 📦 Instalação

Clone este repositório, abra no Visual Studio, compile e execute.

```bash
git clone https://github.com/seu-usuario/SignatorDoc.git
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
   git clone https://github.com/seu-usuario/SignatorDoc.git
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

**[http://www.SignatorDoc.sign.doc](http://www.signatordoc.sign.doc/signator)**  
> *Sistema SignatorDoc.*

---

## 🤝 Contribuições

Contribuições são bem-vindas.  
Abra um **issue** ou envie um **pull request** com sugestões, correções ou melhorias.

---


