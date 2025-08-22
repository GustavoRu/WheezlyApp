SELECT 
    C.Year,
    M.Name Make,
    MD.Name Model,
    SM.Name SubModel,
    Z.ZipCode,
    B.Name BuyerName,
    Q.CurrentAmount CurrentQuote,
    S.Name CurrentStatus,
    SH.StatusDate
FROM Cars C
INNER JOIN Makes M ON C.MakeId = M.Id
INNER JOIN Models MD ON C.ModelId = MD.Id
INNER JOIN SubModels SM ON C.SubModelId = SM.Id
INNER JOIN ZipCodes Z ON C.ZipCodeId = Z.Id
LEFT JOIN Quotes Q ON C.Id = Q.CarId AND Q.IsCurrentQuote = 1
LEFT JOIN Buyers B ON Q.BuyerId = B.Id
LEFT JOIN StatusHistory SH ON C.Id = SH.CarId
LEFT JOIN Status S ON SH.StatusId = S.Id
WHERE C.IsActive = 1 AND C.Id = 3
AND SH.Id = (
    SELECT TOP 1 Id 
    FROM StatusHistory 
    WHERE CarId = C.Id 
    ORDER BY CreatedDate DESC
)