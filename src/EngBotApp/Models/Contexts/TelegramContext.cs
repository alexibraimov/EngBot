using EngBotApp.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngBotApp.Models.Contexts
{
    public class TelegramContext : DbContext
    {
        public DbSet<TelegramUser> Users { get; set; }
        public DbSet<TelegramWord> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelegramWord>()
                .HasOne<TelegramUser>(w => w.TelegramUser)
                .WithMany(u => u.Words)
                .HasForeignKey(u =>u.TelegramUserId);
        }

        public void AddOrUpdate(TelegramUser telegramUser)
        {
            if (Users.FirstOrDefault(x=>x.ChatId == telegramUser.ChatId) == null)
            {
                telegramUser.Words = GetAllWords(telegramUser);
                Users.Add(telegramUser);
            }
        }

        public TelegramContext()
        {
            Database.EnsureCreated();
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Telegram.db");
        }

        public List<TelegramWord> GetAllWords(TelegramUser telegramUser)
        {
            var list = new List<TelegramWord>();

            foreach (var word in WordCollection.All)
            {
                var tWord = new TelegramWord()
                {
                    WordId = word.Id,
                    En = word.En,
                    Ru = word.Ru,
                    IsRemembered = false,
                    TelegramUser = telegramUser
                };
                list.Add(tWord);
                Words.Add(tWord);
            }

            return list;
        }
    }
}
