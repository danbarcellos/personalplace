using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pactor.Infra.DAL.ORM;
using Pactor.Infra.DAL.ORM.Tests.Base;
using PersonalPlace.Domain.Entities.Catalog;

namespace PersonalPlace.Domain.Tests.Entities
{
    [TestClass]
    public class GiveComment : IntegratedBaseIsolatedExecutionTestClass
    {
        #region Test User Values

        private readonly Guid _realtyId = new Guid("AED5BBF5-4F30-4A4E-84AE-54BBECE26C2B");
        private readonly Guid _clientId = new Guid("95F9F8E4-2F30-49C8-821F-DAC984BDE37C");
        private readonly Guid _mentionCommentId = new Guid("EC84F79E-1FCA-4518-A78E-9CBEF16CDDE0");
        private readonly string _text = "Comment text about realty, by client.";
        

        #endregion

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            StartEnvironment();
        }

        [TestMethod]
        [TestCategory("Domain.Entities")]
        [TestCategory("Integrated")]
        public void WhenSaveCommentThenICanRecover()
        {
            //arrange
            var uow = Container.Resolve<IUnitOfWork>();
            var commentRepository = Container.Resolve<IRepository<Comment>>();
            var comment = GetComment();

            //act
            commentRepository.SaveAll(comment);
            uow.Clear();
            var recoveredComment = commentRepository.FindOne(comment.Id);

            //assert
            Assert.IsNotNull(recoveredComment);
            Assert.AreEqual(_text, recoveredComment.Text);
        }

        private Comment GetComment()
        {
            var realty = GetRealty();
            var client = GetClient();
            var mentionComment = GetMentionComment();

            var comment = new Comment(realty, client, mentionComment)
            {
                Text = _text
            };
            return comment;
        }

        private Realty GetRealty()
        {
            var realtyRepository = Container.Resolve<IRepository<Realty>>();
            var realty = realtyRepository.Load(_realtyId);
            return realty;
        }

        private Client GetClient()
        {
            var clientRepository = Container.Resolve<IRepository<Client>>();
            var client = clientRepository.Load(_clientId);
            return client;
        }

        private Comment GetMentionComment()
        {
            var commentRepository = Container.Resolve<IRepository<Comment>>();
            var comment = commentRepository.Load(_mentionCommentId);
            return comment;
        }
    }
}