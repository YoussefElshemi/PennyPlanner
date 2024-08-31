using Core.Interfaces.Repositories;
using Core.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using IMapper = AutoMapper.IMapper;

namespace Infrastructure.Repositories;

public class EmailRepository(
    PennyPlannerDbContext context,
    IMapper mapper) : PagedRepository<EmailMessageOutboxEntity>(context, mapper), IEmailRepository
{
    public Task<bool> ExistsAsync(Guid emailId)
    {
        return context.Emails.AnyAsync(x => x.EmailId == emailId);
    }

    public async Task<EmailMessage> GetAsync(Guid emailId)
    {
        var emailMessageEntity = await context.Emails
            .FirstAsync(x => x.EmailId == emailId);

        return mapper.Map<EmailMessage>(emailMessageEntity);
    }

    public async Task CreateAsync(EmailMessage emailMessage)
    {
        var emailMessageEntity = mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        context.Emails.Add(emailMessageEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailMessage emailMessage)
    {
        var emailMessageToUpdate = mapper.Map<EmailMessageOutboxEntity>(emailMessage);
        var emailMessageEntity = await context.Emails.FirstAsync(x => x.EmailId == emailMessageToUpdate.EmailId);

        emailMessageEntity.IsProcessed = emailMessageToUpdate.IsProcessed;
        emailMessageEntity.UpdatedBy = emailMessageToUpdate.UpdatedBy;
        emailMessageEntity.UpdatedAt = emailMessageToUpdate.UpdatedAt;

        context.Emails.Update(emailMessageEntity);
        await context.SaveChangesAsync();
    }

    public override List<string> GetSortableFields()
    {
        return
        [
            nameof(EmailMessageOutboxEntity.EmailAddress),
            nameof(UserEntity.CreatedAt),
            nameof(UserEntity.UpdatedAt)
        ];
    }

    public override List<string> GetSearchableFields()
    {
        return [];
    }

    public async Task<IEnumerable<EmailMessage>> GetAwaitingEmailsAsync()
    {
        var emailMessageEntities = await context.Emails
            .Where(x => x.IsProcessed == false).ToListAsync();

        return emailMessageEntities.Select(mapper.Map<EmailMessage>);
    }
}