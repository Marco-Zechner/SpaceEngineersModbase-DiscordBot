## What is this Project supposed to do?

### Provide a Discord Bot for my Server

**Use Case Example**

> Player1 has a *Idea* and posts it in the **Mod-Ideas** Formum
> Modder2 sees the Idea and thinks it is great, so he calls the "/adopt" command
Bot workflow:
> /adopt
> - check if a category for Modder2 exists
> - if not, create the category and somewhere save the userID
>   create channels:
>   - news-updates (Text)
>   - chat (Text)
>   - mods (Forum) tags: in-progress, abandoned, released, help-wanted, keen-report
>   - vc (Voice Channel)
> - check if this mod idea (via some kind of ID) is already in their own mods forum
> - if not,
>   - create a new post and somewhere save the mod-idea-postID
>   - tag it as in-progress,
>   - add a link that leads to the mod-idea post
>   - add a embed to the mod-idea that shows who adopted it, what the current state is, etc.
> - if it was already adopted list all users that adopted it. (except abandoned)
>   - /adopt user1
>   - send a collab request to user1
>   - /adopt new
>   - adopt the mod like it was never adopted before
