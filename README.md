# Switch-case string analyzer
A very simple analyzer that reports switch-case statements on `string`s as a warning.

### Why?
If you are switching on a string you most likely want to re-think your domain model. 
Not saying that it _never_ should be done, but in that case, I would rather like to explicitly say 'yes, it is okay'.