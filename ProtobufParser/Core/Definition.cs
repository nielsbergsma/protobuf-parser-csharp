using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufParser.Core
{
    public class Definition
    {
        private readonly SyntaxVersion syntaxVersion;
        private readonly List<Import> imports;
        private readonly List<Message> messages;
        private readonly List<Enumeration> enumerations;
        private readonly List<Option> options;
        private Package package;

        public Definition()
        {
            this.syntaxVersion = SyntaxVersion.Proto3;
            this.imports = new List<Import>();
            this.messages = new List<Message>();
            this.enumerations = new List<Enumeration>();
            this.options = new List<Option>();
            this.package = Package.Undefined;
        }

        public SyntaxVersion SyntaxVersion
        {
            get { return syntaxVersion; }
        }

        public IEnumerable<Import> Imports
        {
            get { return imports; }
        }

        public IEnumerable<Message> Messages
        {
            get { return messages; }
        }

        public IEnumerable<Enumeration> Enumerations
        {
            get { return enumerations; }
        }

        public IEnumerable<Option> Options
        {
            get { return options; }
        }

        public Package Package
        {
            get { return package; }
        }

        public void Set(Package package)
        {
            this.package = package;
        }

        public void Set(Option option)
        {
            options.Add(option);
        }

        public void Add(Import import)
        {
            imports.Add(import);
        }

        public void Add(Message message)
        {
            messages.Add(message);
        }

        public void Add(Enumeration enumeration)
        {
            enumerations.Add(enumeration);
        }

        public void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
