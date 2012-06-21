function [ Adj,Result ] = DevMatrix( M,s )
% By Abdulrhman Alkhanifer
% This function accepts a number array with developers who worked 
% on every review and calulates their ajd matrix with communication
% probablities

%create results matrix (dev_num X dev_num) and intialize it
Result=zeros(s,s);
temp=zeros(s,s);
Adj=zeros(s,s);
[r,c]=size(M);
%loop on the reviewers matrix (input matrix)
for i=1:r
    for j=1:c
        % if M[i,j]==0 then there's no dev in that field!
        if M(i,j)~=0
            dev1=M(i,j);
            for t=j+1:c
                if M(i,t)~=0
                    temp(dev1,M(i,t))=temp(dev1,M(i,t))+1;
                    temp(M(i,t),dev1)=temp(M(i,t),dev1)+1;
                                        
                end
                
            end
            
        end
        
    end
end %end for i

%now we'll calculate the probablities
for i=1:s
    
    for j=1:s
        row_total=sum(temp(i,:));
        
        if temp(i,j)~=0
            Result(i,j)=temp(i,j)/row_total;
            Adj(i,j)=1;
        end
    end
    
end


end

