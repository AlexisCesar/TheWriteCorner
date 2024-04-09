local jwt = require "resty.jwt"
local validators = require "resty.jwt-validators"

local token_secret = "9EkdX4IwbqZPScUzFNZpB7OIJkMlWJf9ceYfbZgHqt8="

if ngx.var.request_method ~= "OPTIONS" then
    local auth_header = ngx.var.http_Authorization
    
    if auth_header == nil then
      ngx.status = ngx.HTTP_UNAUTHORIZED
      ngx.header.content_type = "application/json; charset=utf-8"
      ngx.say('{"error": "Forbidden - Token not informed"}')
      ngx.exit(ngx.HTTP_UNAUTHORIZED)
    end

    local _, _, token = string.find(auth_header, "Bearer%s+(.+)")
    
    if token == nil then
      ngx.status = ngx.HTTP_UNAUTHORIZED
      ngx.header.content_type = "application/json; charset=utf-8"
      ngx.say('{"error": "Forbidden - Token not informed"}')
      ngx.exit(ngx.HTTP_UNAUTHORIZED)
    end
 
    local claim_spec = {
       exp = validators.is_not_expired() -- To check expiry
    }

    local jwt_obj = jwt:verify(token_secret, token, claim_spec)

    if not jwt_obj["verified"] then    
      ngx.log(ngx.STDERR, "Token is invalid " .. tostring(jwt_obj["verified"]))
      ngx.status = ngx.HTTP_UNAUTHORIZED
      ngx.header.content_type = "application/json; charset=utf-8"
      ngx.say("Token is invalid due to: " .. jwt_obj.reason)
      ngx.exit(ngx.HTTP_UNAUTHORIZED)
    end
end